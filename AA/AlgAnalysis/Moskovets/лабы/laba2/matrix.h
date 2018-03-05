#ifndef MATRIX_H
#define MATRIX_H

#define RAND_RANGE 100
#define EPS 0.0001

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
    friend Matrix multMod2Vinograd(const Matrix &a, const Matrix &b);
};
Matrix multStandart(const Matrix &a, const Matrix &b);
Matrix multVinograd(const Matrix &a, const Matrix &b);
Matrix multModVinograd(const Matrix &a, const Matrix &b);
Matrix multMod2Vinograd(const Matrix &a, const Matrix &b);

#endif // MATRIX_H
