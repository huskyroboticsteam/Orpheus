#include <cstdlib>
#include <iostream>
#include <complex>

#include <kalman/kvector.hpp>

using namespace std;
using namespace Kalman;

template <typename T, K_UINT_32 BEG, bool DBG>
void test() {

  typedef KVector<T, BEG, DBG> Vector;
  T values[5] = {5.0, 4.0, 3.0, 2.0, 1.0};

  // Constructor tests.
  Vector v0;
  Vector v1(0);
  Vector v2(5);
  Vector v3(0, T(1.0));
  Vector v4(4, T(2.0));
  Vector v5(0, values);
  Vector v6(3, values);
  Vector v7(v5);
  Vector v8(v6);

  cout << "v0 : " << v0 << endl;
  cout << "v1 : " << v1 << endl;
  cout << "v2 : " << v2 << endl;
  cout << "v3 : " << v3 << endl;
  cout << "v4 : " << v4 << endl;
  cout << "v5 : " << v5 << endl;
  cout << "v6 : " << v6 << endl;
  cout << "v7 : " << v7 << endl;
  cout << "v8 : " << v8 << endl << endl;

  // Tests for operator=(const T&) and resize().
  v4.resize(0);
  v4 = T(-1.0);
  v5 = T(6.0);
  v6 = T(7.0);
  v7.resize(2);
  v7 = T(8.0);
  v8.resize(6);
  v8 = T(9.0);
  
  cout << "v4 : " << v4 << endl;
  cout << "v5 : " << v5 << endl;
  cout << "v6 : " << v6 << endl;
  cout << "v7 : " << v7 << endl;
  cout << "v8 : " << v8 << endl << endl;

  // Tests for assignement operator and assign().
  // Tests swap() at the same time, under the covers.
  v5 = v7;
  v6 = v8;
  v7 = v0;
  v0.assign(0, values);
  v1.assign(2, values);
  v2.assign(0, values);
  v4.assign(3, values);
  
  cout << "v0 : " << v0 << endl;
  cout << "v1 : " << v1 << endl;
  cout << "v2 : " << v2 << endl;
  cout << "v3 : " << v3 << endl;
  cout << "v4 : " << v4 << endl;
  cout << "v5 : " << v5 << endl;
  cout << "v6 : " << v6 << endl;
  cout << "v7 : " << v7 << endl;
  cout << "v8 : " << v8 << endl << endl;
  
  // Tests member access functions.
  // Tests exceptions at the same time.
  const Vector& ref = v4;

  cout << "v4(1)  = " << v4(1)  << endl;
  cout << "ref(1) = " << ref(1) << endl;

  v4(1) = T(3.0);
  // ref(1) = T(2.0);  error !!!
  
  cout << "v4(1)  = " << v4(1)  << endl;
  cout << "ref(1) = " << ref(1) << endl;

  if (DBG) {

    try {
      v4(100) = T(7.0);
    } catch(OutOfBoundError& e) {
      cout << e.what() << endl;
    }

    try {
      cout << ref(100) << endl;
    } catch(OutOfBoundError& e) {
      cout << e.what() << endl;
    }

  }

  cout << endl;

  // Output has already been tested extensively.
  // Let's test input.
  // This also tests get() and put() under the covers.
  cout << "v0 is of size : " << v0.size() << endl;
  cin >> v0;
  cout << "v0 : " << v0 << endl;

  cout << "v1 is of size : " << v1.size() << endl;
  cin >> v1;
  cout << "v1 : " << v1 << endl;

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
