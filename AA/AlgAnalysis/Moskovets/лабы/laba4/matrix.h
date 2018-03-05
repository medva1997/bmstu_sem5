#ifndef MATRIX_H
#define MATRIX_H

#define RAND_RANGE 100
#define EPS 0.0001

#include <thread>
#include <functional>

typedef double elemType;

class Matrix
{
private:
    elemType** arr;
    int row;
    int col;
public:
    Matrix(int n, int m);
    Matrix(const Matrix& obj);
    Matrix(Matrix &&obj);
    void generateRandomElements();
    ~Matrix();

    void print();

    bool operator==(const Matrix &obj);

    friend Matrix multStandart(const Matrix &a, const Matrix &b);
    friend Matrix multVinograd(const Matrix &a, const Matrix &b);
    friend Matrix multModVinograd(const Matrix &a, const Matrix &b);
    friend Matrix multThreadVinograd(const Matrix &a, const Matrix &b, int count);
    friend Matrix multMod2Vinograd(const Matrix &a, const Matrix &b);
    friend void funcThread(const Matrix &a, const Matrix &b, Matrix &res,  elemType* rowFactor,  elemType* colFactor, int num, int count);
   // friend void funcTrash(const Matrix &a, const Matrix &b, Matrix &res,  int num, int count);
};
Matrix multStandart(const Matrix &a, const Matrix &b);
Matrix multVinograd(const Matrix &a, const Matrix &b);
Matrix multModVinograd(const Matrix &a, const Matrix &b);
Matrix multMod2Vinograd(const Matrix &a, const Matrix &b);
Matrix multThreadVinograd(const Matrix &a, const Matrix &b, int count);
void funcThread(const Matrix &a, const Matrix &b, Matrix &res, elemType* rowFactor,  elemType* colFactor, int num, int count);

#endif // MATRIX_H
