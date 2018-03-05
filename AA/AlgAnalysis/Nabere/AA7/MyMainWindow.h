#pragma once

#include <QtWidgets/QMainWindow>
#include "ui_MyMainWindow.h"
#include <fstream>
#include <stdlib.h>
#include <time.h>
#include <math.h>
#include <iostream>
#include <assert.h>
#include <qtimer.h>
#include <qdebug.h>



class MyMainWindow : public QMainWindow
{
	Q_OBJECT

public:
	MyMainWindow(QWidget *parent = Q_NULLPTR);
	QGraphicsScene *Scene;

	void StartProcess();
	void DrawTown();
	void DrawLines();
	void UpdatePicture();
	void restartAnts();
private:
	Ui::MyMainWindowClass ui;

	public slots:
void on_pushButton_clicked();
};


