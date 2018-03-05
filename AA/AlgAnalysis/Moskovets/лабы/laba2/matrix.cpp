#include "matrix.h"
#include <stdlib.h>
#include "math.h"
#include <assert.h>
#include <memory.h>
#include <iostream>

Matrix::Matrix(int n, int m)
{
    row = n;
    col = m;
    arr = new elemType*[n];
    for (int i = 0; i < n; i++) {
        arr[i] = new elemType[m];
    }
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < m; j++) {
            arr[i][j] = 0;
        }
    }
}

Matrix::Matrix(const Matrix &obj)
{
    arr = new elemType*[obj.row];
    for (int i = 0; i < row; i++) {
        arr[i] = new elemType[obj.col];
        memcpy(arr[i], obj.arr[i], obj.col * sizeof(elemType));
    }
    col = obj.col;
    row = obj.row;
}

Matrix::Matrix(Matrix && obj)
{
    arr = obj.arr;
    col = obj.col;
    row = obj.row;

    obj.arr = nullptr;
    obj.col = 0;
    obj.row = 0;
}

void Matrix::generateRandomElements()
{
    for(int i = 0; i < row; i++) {
        for(int j = 0; j < col; j++) {
            arr[i][j] = ((elemType) rand() / RAND_MAX) * RAND_RANGE;
        }
    }
}

Matrix::~Matrix()
{
    for (int i = 0; i < row; i++) {
        delete[] arr[i];
    }
    delete[] arr;
}

void Matrix::print()
{
    for(int i = 0; i < row; i++) {
        for(int j = 0; j < col; j++) {
            std::cout << arr[i][j] << " ";
        }
        std::cout << std::endl;
    }
}

bool Matrix::operator==(const Matrix &obj)
{
    for(int i = 0; i < row; i++) {
        for(int j = 0; j < col; j++) {
            if(fabs(arr[i][j] - obj.arr[i][j]) > EPS)
                return false;
        }
    }
    return true;
}

Matrix multStandart(const Matrix &a, const Matrix &b)
{
    assert(a.col == b.row);

    Matrix res(a.row, b.col);

    for(int i = 0; i < a.row; i++) {
        for(int j = 0; j < b.col; j++) {
            for(int k = 0; k < b.row; k++) {
                res.arr[i][j] += a.arr[i][k] * b.arr[k][j];
            }
        }
    }
    return res;
}

Matrix multVinograd(const Matrix &a, const Matrix &b)
{
    assert(a.col == b.row);

    Matrix res(a.row, b.col);

    elemType rowFactor[a.row];
    elemType colFactor[b.col];

    for(int i = 0; i < a.row; i++) {
        rowFactor[i] = 0;
        for(int j = 0; j < a.col / 2; j++) {
            rowFactor[i] = rowFactor[i] + a.arr[i][2 * j + 1] * a.arr[i][2 * j];
        }
    }

    for(int i = 0; i < b.col; i++) {
        colFactor[i] = 0;
        for(int j = 0; j < a.col / 2; j++) {
            colFactor[i] = colFactor[i] + b.arr[2 * j + 1][i] * b.arr[2 * j][i];
        }
    }

    for(int i = 0; i < a.row; i++) {
        for(int j = 0; j < b.col; j++) {
            res.arr[i][j] = -rowFactor[i] - colFactor[j];
            for(int k = 0; k < a.col / 2; k++) {
                res.arr[i][j] = res.arr[i][j] + (a.arr[i][2*k+1] + b.arr[2*k][j]) *
                                                (a.arr[i][2*k] + b.arr[2*k+1][j]);
            }
        }
    }
    if(a.col % 2 == 1) {
        for(int i = 0; i < a.row; i++)
            for(int j = 0; j < b.col; j++)
                res.arr[i][j] = res.arr[i][j] + a.arr[i][a.col-1] * b.arr[a.col-1][j];
    }

    return res;
}

Matrix multMod2Vinograd(const Matrix &a, const Matrix &b)
{
    assert(a.col == b.row);

    Matrix res(a.row, b.col);

    elemType rowFactor[a.row];
    elemType colFactor[b.col];

    int mid = a.col / 2; //2

    // f1 = 2 + 11*n + 12mid
    for(int i = 0; i < a.row; i++) { // 2 + n(2 + 9 + 12mid)
        rowFactor[i] = a.arr[i][0] * a.arr[i][1]; //7
        for(int j = 1; j < mid; j++) { // 2 + 12mid
            rowFactor[i] += a.arr[i][2 * j + 1] * a.arr[i][2 * j]; // 10
        }
    }
    // f1 = 2 + 11*k + 12mid
    for(int i = 0; i < b.col; i++) {  //2 + 6*b.col + 15*a.col*b.col / 2
        colFactor[i] = b.arr[0][i] * b.arr[1][i];
        for(int j = 1; j < mid; j++) {
            colFactor[i] += b.arr[2 * j + 1][i] * b.arr[2 * j][i];
        }
    }

    //f3 = 2 + 2n + 8nk + 22nk*mid
    for(int i = 0; i < a.row; i++) { // 2 + n(2 + k(8 + 22mid))
        for(int j = 0; j < b.col; j++) { //2 + k(8 + 22mid)
            res.arr[i][j] -= (rowFactor[i] + colFactor[j]); //6
            for(int k = 0; k < mid; k++) { //2 + 22mid
                res.arr[i][j] += (a.arr[i][2*k+1] + b.arr[2*k][j]) *
                                 (a.arr[i][2*k] + b.arr[2*k+1][j]); //20
            }
        }
    }

    int last = a.col - 1; //2
    if(a.col % 2) { //1
        //2 + 4n + 10nk
        for(int i = 0; i < a.row; i++) // 2 + n(4 + 10k)
            for(int j = 0; j < b.col; j++) //2 + 10k
                res.arr[i][j] += a.arr[i][last] * b.arr[last][j]; //8
    }

    return res;
}
Matrix multModVinograd(const Matrix &a, const Matrix &b)
{
    assert(a.col == b.row);

    int ind;
    elemType tmp;
    Matrix res(a.row, b.col);

    elemType rowFactor[a.row];
    elemType colFactor[b.col];

    //int mid = a.col / 2;

    int last = a.col - 1; //2

    //f1 = 4 + 9n + 9k
    for(int i = 0; i < a.row; i++) { //2 + a.row*(2+7)
        rowFactor[i] = a.arr[i][0] * a.arr[i][1]; //7
    }
    for(int i = 0; i < b.col; i++) {
        colFactor[i] = b.arr[0][i] * b.arr[1][i];
    }

    //f2 = 2 + mid(6 + 9n + 9k) - (6 + 9n + 9k)
    //f2 = mid(6 + 9n + 9k) - 9n - 9k - 4
    for(int j = 2; j < last; j += 2) { //2 + (mid-1)(6 + 9n + 9k)
        ind = j + 1; //2
        for(int i = 0; i < a.row; i++) { //2 + n(7 + 2)
            rowFactor[i] += a.arr[i][j] * a.arr[i][ind]; // 7
        }
        for(int i = 0; i < b.col; i++) {
            colFactor[i] += b.arr[j][i] * b.arr[ind][i];
        }
    }
    //f1 + f2 = 2 + mid(6 + 9n + 9k)

    //f3(bad)  = 1 + 2 + n(2 + k(18 + 16mid)) = 3 + 2n + 18nk + 16nk*mid
    //f3(good) = 1 + 2 + n(2 + k(12 + 16mid)) = 3 + 2n + 12nk + 16nk*mid
    if(a.col % 2) { //1
        for(int i = 0; i < a.row; i++) { // 2 + n(2 + k(18 + 16mid))
            for(int j = 0; j < b.col; j++) { //2 + k(18 + 16mid)
                tmp = -(rowFactor[i] + colFactor[j]); //5
                for(int k = 0; k < last; k += 2) { //2 + 16mid
                    tmp += (a.arr[i][k+1] + b.arr[k][j]) *
                           (a.arr[i][k]   + b.arr[k+1][j]); //14
                }
                tmp += a.arr[i][last] * b.arr[last][j]; //6
                res.arr[i][j] = tmp; //3
            }
        }
    } else {
        for(int i = 0; i < a.row; i++) { // 2 + n(2 + k(12 + 16mid))
            for(int j = 0; j < b.col; j++) { //2 + k(12 + 16mid)
                tmp = -(rowFactor[i] + colFactor[j]); //5
                for(int k = 0; k < last; k += 2) { //2 + 16mid
                    tmp += (a.arr[i][k+1] + b.arr[k][j]) *
                           (a.arr[i][k]   + b.arr[k+1][j]); //14
                }
                res.arr[i][j] = tmp; // 3
            }
        }
    }

    return res;
}
