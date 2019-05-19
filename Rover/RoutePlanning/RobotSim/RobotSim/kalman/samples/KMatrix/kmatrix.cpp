#include <cstdlib>
#include <iostream>
#include <complex>

#include <kalman/kmatrix.hpp>

using namespace std;
using namespace Kalman;

template <typename T, K_UINT_32 BEG, bool DBG>
void test() {

  typedef KMatrix<T, BEG, DBG> Matrix;
  T values[10] = {9.0, 8.0, 7.0, 6.0, 5.0, 4.0, 3.0, 2.0, 1.0, 0.0};

  // Constructor tests.
  Matrix M0;
  Matrix M1(0,4);
  Matrix M2(2,3);
  Matrix M3(0, 2, T(1.0));
  Matrix M4(4, 1, T(2.0));
  Matrix M5(3, 0, values);
  Matrix M6(3, 1, values);
  Matrix M7(M5);
  Matrix M8(M6);
  Matrix M9(4,0);

  cout << "M0 : " << M0 << endl;
  cout << "M1 : " << M1 << endl;
  cout << "M2 : " << M2 << endl;
  cout << "M3 : " << M3 << endl;
  cout << "M4 : " << M4 << endl;
  cout << "M5 : " << M5 << endl;
  cout << "M6 : " << M6 << endl;
  cout << "M7 : " << M7 << endl;
  cout << "M8 : " << M8 << endl;
  cout << "M9 : " << M9 << endl << endl;

  // Tests for operator=(const T&) and resize().
  M4.resize(0, 2);
  M4 = T(-1.0);
  M5 = T(6.0);
  M6 = T(7.0);
  M7.resize(2, 1);
  M7 = T(8.0);
  M8.resize(4, 2);
  M8 = T(9.0);
  
  cout << "M4 : " << M4 << endl;
  cout << "M5 : " << M5 << endl;
  cout << "M6 : " << M6 << endl;
  cout << "M7 : " << M7 << endl;
  cout << "M8 : " << M8 << endl << endl;

  // Tests for assignement operator and assign().
  // Tests swap() at the same time, under the covers.
  M5 = M7;
  M6 = M8;
  M7 = M0;
  M0.assign(0, 2, values);
  M1.assign(2, 1, values);
  M2.assign(3, 0, values);
  M4.assign(3, 2, values);
  
  cout << "M0 : " << M0 << endl;
  cout << "M1 : " << M1 << endl;
  cout << "M2 : " << M2 << endl;
  cout << "M3 : " << M3 << endl;
  cout << "M4 : " << M4 << endl;
  cout << "M5 : " << M5 << endl;
  cout << "M6 : " << M6 << endl;
  cout << "M7 : " << M7 << endl;
  cout << "M8 : " << M8 << endl << endl;
  
  // Tests member access functions.
  // Tests exceptions at the same time.
  const Matrix& ref = M4;

  cout << "M4(1,1)  = " << M4(1,1)  << endl;
  cout << "ref(1,1) = " << ref(1,1) << endl;

  M4(1,1) = T(3.0);
  // ref(1,1) = T(2.0);  error !!!
  
  cout << "M4(1,1)  = " << M4(1,1)  << endl;
  cout << "ref(1,1) = " << ref(1,1) << endl;

  if (DBG) {

    try {
      M4(100,100) = T(7.0);
    } catch(OutOfBoundError& e) {
      cout << e.what() << endl;
    }

    try {
      cout << ref(100,100) << endl;
    } catch(OutOfBoundError& e) {
      cout << e.what() << endl;
    }

  }

  cout << endl;

  // Output has already been tested extensively.
  // Let's test input.
  // This also tests get() and put() under the covers.
  cout << "M0 is of size : " << M0.nrow() << " by " << M0.ncol() << endl;
  cin >> M0;
  cout << "M0 : " << M0 << endl;

  cout << "M1 is of size : " << M1.nrow() << " by " << M1.ncol() << endl;
  cin >> M1;
  cout << "M1 : " << M1 << endl;

  cout << "----------------------------" << endl << endl;

}

int main() {
  test<float, 0, true>();
  test<float, 0, false>();
  test<float, 1, true>();
  test<float, 1, false>();
  test<double, 0, true>();
  test<double, 0, false>();
  test<double, 1, true>();
  test<double, 1, false>();
  test<complex<double>, 0, true>();
  test<complex<double>, 0, false>();
  test<complex<double>, 1, true>();
  test<complex<double>, 1, false>();
  return 0;
}
