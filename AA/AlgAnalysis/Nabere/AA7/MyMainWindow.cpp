#include "MyMainWindow.h"


#define MAX_CITIES 60
#define MAX_DIST 400
#define MAX_TOUR (MAX_CITIES * MAX_DIST)
#define MAX_ANTS 30


struct cityType {
	int x, y;
};

struct antType {

	int curCity, nextCity, pathIndex;
	int tabu[MAX_CITIES];
	int path[MAX_CITIES];
	double tourLength;
};

#define ALPHA 1.0
#define BETA 1.0 
#define RHO 0.9 
#define QVAL 100
#define MAX_TOURS 20
#define MAX_TIME (MAX_TOURS * MAX_CITIES)
#define INIT_PHER (1.0/MAX_CITIES)


cityType cities[MAX_CITIES];
antType ants[MAX_ANTS];

double dist[MAX_CITIES][MAX_CITIES];

double phero[MAX_CITIES][MAX_CITIES];

double best = (double)MAX_TOUR;
int bestIndex;


void init()
{
	int from, to, ant;

	//creating cities

	for (from = 0; from < MAX_CITIES; from++)
	{
		//randomly place cities

		cities[from].x = rand() % MAX_DIST;

		cities[from].y = rand() % MAX_DIST;
		//printf("\n %d %d",cities[from].x, cities[from].y);
		for (to = 0; to<MAX_CITIES; to++)
		{
			dist[from][to] = 0.0;
			phero[from][to] = INIT_PHER;
		}
	}

	//computing distance

	for (from = 0; from < MAX_CITIES; from++)
	{
		for (to = 0; to < MAX_CITIES; to++)
		{
			if (to != from && dist[from][to] == 0.0)
			{
				int xd = pow(abs(cities[from].x - cities[to].x), 2);
				int yd = pow(abs(cities[from].y - cities[to].y), 2);

				dist[from][to] = sqrt(xd + yd);
				dist[to][from] = dist[from][to];

			}
		}
	}

	

	to = 0;
	for (ant = 0; ant < MAX_ANTS; ant++)
	{
		if (to == MAX_CITIES)
			to = 0;

		ants[ant].curCity = to++;

		for (from = 0; from < MAX_CITIES; from++)
		{
			ants[ant].tabu[from] = 0;
			ants[ant].path[from] = -1;
		}

		ants[ant].pathIndex = 1;
		ants[ant].path[0] = ants[ant].curCity;
		ants[ant].nextCity = -1;
		ants[ant].tourLength = 0;

		//loading first city into tabu list

		ants[ant].tabu[ants[ant].curCity] = 1;

	}
}

void MyMainWindow::restartAnts()
{
	int ant, i, to = 0;

	for (ant = 0; ant<MAX_ANTS; ant++)
	{
		if (ants[ant].tourLength < best)
		{
			best = ants[ant].tourLength;
			bestIndex = ant;
			UpdatePicture();
			qDebug() << best;
		}

		ants[ant].nextCity = -1;
		ants[ant].tourLength = 0.0;

		for (i = 0; i<MAX_CITIES; i++)
		{
			ants[ant].tabu[i] = 0;
			ants[ant].path[i] = -1;
		}

		if (to == MAX_CITIES)
			to = 0;

		ants[ant].curCity = to++;

		ants[ant].pathIndex = 1;
		ants[ant].path[0] = ants[ant].curCity;

		ants[ant].tabu[ants[ant].curCity] = 1;
	}
}


double antProduct(int from, int to)
{
	return((pow(phero[from][to], ALPHA) * pow((1.0 / dist[from][to]), BETA)));
}

int selectNextCity(int ant)
{
	int from, to;
	double denom = 0.0;

	from = ants[ant].curCity;

	for (to = 0; to < MAX_CITIES; to++) { if (ants[ant].tabu[to] == 0) { denom += antProduct(from, to); } }       assert(denom != 0.0);       do {
		double p;       to++;               if (to >= MAX_CITIES)
			to = 0;
		if (ants[ant].tabu[to] == 0)
		{
			p = antProduct(from, to) / denom;

			double x = ((double)rand() / RAND_MAX);
			if (x < p)
			{

				break;
			}
		}
	} while (1);

	return to;
}

int simulateAnts()
{
	int k;
	int moving = 0;

	for (k = 0; k<MAX_ANTS; k++)
	{
		

		if (ants[k].pathIndex < MAX_CITIES) {
			ants[k].nextCity = selectNextCity(k);
			ants[k].tabu[ants[k].nextCity] = 1;        
			ants[k].path[ants[k].pathIndex++] = ants[k].nextCity; 
			ants[k].tourLength += dist[ants[k].curCity][ants[k].nextCity];

			if (ants[k].pathIndex == MAX_CITIES)
			{
				ants[k].tourLength += dist[ants[k].path[MAX_CITIES - 1]][ants[k].path[0]];
			}

			ants[k].curCity = ants[k].nextCity;
			moving++;

		}
	}

	return moving;
}



void updateTrails()
{
	int from, to, i, ant;

	

	for (from = 0; from<MAX_CITIES; from++)
	{
		for (to = 0; to<MAX_CITIES; to++)
		{
			if (from != to)
			{
				phero[from][to] *= (1.0 - RHO);

				if (phero[from][to]<0.0)
				{
					phero[from][to] = INIT_PHER;
				}
			}
		}
	}

	

	for (ant = 0; ant<MAX_ANTS; ant++)
	{
		for (i = 0; i<MAX_CITIES; i++)
		{
			if (i < MAX_CITIES - 1)
			{
				from = ants[ant].path[i];
				to = ants[ant].path[i + 1];
			}
			else
			{
				from = ants[ant].path[i];
				to = ants[ant].path[0];
			}

			phero[from][to] += (QVAL / ants[ant].tourLength);
			phero[to][from] = phero[from][to];

		}
	}

	for (from = 0; from < MAX_CITIES; from++)
	{
		for (to = 0; to<MAX_CITIES; to++)
		{
			phero[from][to] *= RHO;
		}
	}

}
















MyMainWindow::MyMainWindow(QWidget *parent)
	: QMainWindow(parent)
{
	ui.setupUi(this);
	Scene = new QGraphicsScene();
	Scene->setSceneRect(0, 0, ui.graphicsView->width(), ui.graphicsView->height());
	ui.graphicsView->setScene(Scene);

	
	
}

void MyMainWindow::StartProcess() {
	int curTime = 0;

	//srand(time(NULL));

	init();
	int time = 6000;
	while (curTime <  time)
	{
		curTime++;
		ui.lcdNumber->display(curTime);
		QEventLoop loop; 
		QTimer::singleShot(1, &loop, SLOT(quit())); 
		loop.exec();

		if (simulateAnts() == 0)
		{
			updateTrails();

			if (curTime != MAX_TIME)
				restartAnts();


		}
	}


	
}

void MyMainWindow::DrawTown() {
	int N = MAX_CITIES;
	int R = 4;
	for (int i = 0; i < N; i++) {
		Scene->addEllipse(cities[i].x - R / 2, cities[i].y - R / 2, R, R);
	}
}

void MyMainWindow::DrawLines() {
	int *P = ants[bestIndex].path;
	int N = MAX_CITIES;
	for (int i = 0; i < N; i++) {
		int j = (i + 1) % N;
		Scene->addLine(cities[P[i]].x, cities[P[i]].y, cities[P[j]].x, cities[P[j]].y,QPen(QColor(0,0,255)));
	}
}

void MyMainWindow::UpdatePicture() {
	Scene->clear();
	DrawTown();
	DrawLines();
}

void MyMainWindow::on_pushButton_clicked() {
	StartProcess();
}