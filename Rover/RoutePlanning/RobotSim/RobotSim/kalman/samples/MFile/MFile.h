// This file is part of kfilter.
// kfilter is a C++ variable-dimension extended kalman filter library.
//
// Copyright (C) 2004        Vincent Zalzal, Sylvain Marleau
// Copyright (C) 2001, 2004  Richard Gourdeau
// Copyright (C) 2004        GRPR and DGE's Automation sector
//                           École Polytechnique de Montréal
//
// Code adapted from algorithms presented in :
//      Bierman, G. J. "Factorization Methods for Discrete Sequential
//      Estimation", Academic Press, 1977.
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

#ifndef MFILE_H
#define MFILE_H

#include <vector>
#include <string>
#include "kalman/ktypes.hpp"
#include "kalman/kvector.hpp"
#include "kalman/kmatrix.hpp"

namespace Kalman {

#define LINE_MAX_LENGTH 65536
#define ROW_VECTOR 0
#define COLUMN_VECTOR 1

struct MFileElement
{
  unsigned int Index;
  unsigned int Rows;
  unsigned int Cols;
  std::string Name;
  MFileElement();
  MFileElement(const MFileElement& tmp);
  ~MFileElement();
  MFileElement& operator=(const MFileElement& tmp);
};


class MFile {
 public:

  MFile();
  ~MFile();

  int read(char *filename);
  int save(char *filename);
  void print();

template<typename T, K_UINT_32 BEG, bool DBG>
inline int get(std::string name, Kalman::KVector<T,BEG,DBG>& tmpvector);

template<typename T, K_UINT_32 BEG, bool DBG>
  inline int get(std::string name, Kalman::KMatrix<T,BEG,DBG>& tmpmatrix);

template<typename T, K_UINT_32 BEG, bool DBG>
  inline int add(std::string name, Kalman::KVector<T,BEG,DBG>& tmpvector, int type=ROW_VECTOR);

template<typename T, K_UINT_32 BEG, bool DBG>
  inline int add(std::string name, Kalman::KMatrix<T,BEG,DBG>& tmpmatrix);

 private:

  bool add_double(std::string &tmpstr);

  std::vector<MFileElement> VectorMFileElement;
  std::vector<double> Data;
};

}

#include "MFile_impl.hpp"

#endif
