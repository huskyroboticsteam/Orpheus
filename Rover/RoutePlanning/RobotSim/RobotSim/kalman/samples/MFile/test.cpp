#include "stdlib.h"
#include "stdio.h"

#include "MFile.h"
#include "kalman/ktypes.hpp"
#include "kalman/kvector.hpp"
#include "kalman/kmatrix.hpp"

using namespace Kalman;
using namespace std;

int main()
{
  MFile mfile;
  int i,j;

  KVector<double, 1, true> T, tmpVec;
  KMatrix<double, 1, true> ReperesX, tmpMat;

  // Matlab format
  selectKVectorContext(createKVectorContext(" ", "[ ", " ];", 4));
  selectKMatrixContext(createKMatrixContext(" ", " ;\n  ", "[ ", " ];", 4));
  mfile.read("test.m");

  mfile.print();

  mfile.get("T", T);

  mfile.get("ReperesX", ReperesX);

  cout<<T<<endl<<endl;

  cout<<ReperesX<<endl<<endl;

  tmpVec.resize(10);

  for(i=1; i<=10; i++)
    {
      tmpVec(i)=i;
    }

  tmpMat.resize(2,5);

  for(i=1; i<=2; i++)
    {
      for(j=1; j<=5; j++)
		{
		  tmpMat(i,j)=i*5+j;
		}
    }

  mfile.add("tmpVec", tmpVec);

  mfile.add("tmpVec2", tmpVec, COLUMN_VECTOR);

  mfile.add("tmpMat", tmpMat);

  mfile.print();

  mfile.save("test2.m");

  return 0;
}
