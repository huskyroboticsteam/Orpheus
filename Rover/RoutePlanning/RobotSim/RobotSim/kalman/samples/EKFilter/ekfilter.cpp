#include <kalman/ekfilter.hpp>

using namespace std;
using namespace Kalman;

typedef EKFilter<double, 1, false, false, true> MyFilter;

class A : public MyFilter {
protected:
  void makeMeasure() {}
  void makeProcess() {}
};

//int main() {}

int main() {
  A filter;
  return 0;
}
