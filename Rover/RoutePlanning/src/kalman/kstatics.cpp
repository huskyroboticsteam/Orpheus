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

//! \file
//! \brief Contains the implementation for printing contexts

#include <vector>

#include "kalman/kvector.hpp"
#include "kalman/kmatrix.hpp"

using namespace std;
using namespace Kalman;

namespace {

  //! Type used to store all the possible vector printing contexts.
  typedef vector<KVectorContextImpl> VectorContextList;

  //! The global list of vector contexts.
  VectorContextList vectorContexts(1, KVectorContextImpl());

  //! The global index of the currently selected vector context.
  KVectorContext currentVectorIndex = 0;

  //! Type used to store all the possible matrix printing contexts.
  typedef vector<KMatrixContextImpl> MatrixContextList;

  //! The global list of matrix contexts.
  MatrixContextList matrixContexts(1, KMatrixContextImpl());

  //! The global index of the currently selected matrix context.
  KMatrixContext currentMatrixIndex = 0;

}

//! Global pointer to the currently selected vector printing context. Never 0.
KVectorContextImpl* Kalman::currentVectorContext = &vectorContexts[0];

//! Global index of the default vector printing context.
KVectorContext Kalman::DEFAULT_VECTOR_CONTEXT = 0;

//! This function creates a new vector printing context and adds it to the
//! global list of vector contexts. In the discussion below, a whitespace
//! refers to a single character among the following : a horizontal space, 
//! a horizontal tabulation, a linefeed or a carriage return.
//! \warning Severe constraints are imposed on delimiter strings. Pay
//!   attention to them, or else, weird bugs could happen !
//! \param elemDelim Delimiter string between vector elements. Must not be
//!   empty. Must start and end with one or more whitespace(s). Cannot contain
//!   a middle whitespace (a lone whitespace which is not at the beginning, 
//!   nor at the end of the string).
//! \param startDelim Starting string before first vector element. May be
//!   empty. If not empty, must not begin with a whitespace and must end
//!   with one or more whitespace(s). Cannot contain a middle whitespace.
//! \param endDelim Ending string after last vector element. May be
//!   empty. If not empty, must not end with a whitespace and must begin
//!   with one or more whitespace(s). Cannot contain a middle whitespace
//! \param prec Number of significant digits to output. Must be between 1 and
//!   9, or else, will be clipped.
//! \return A handle to the newly created vector context.
KVectorContext Kalman::createKVectorContext(std::string elemDelim, 
                                            std::string startDelim,
                                            std::string endDelim,
                                            unsigned prec) {
  if (prec < 1) prec = 1;
  if (prec > 9) prec = 9;

  vectorContexts.push_back(KVectorContextImpl(elemDelim, 
                                              startDelim, 
                                              endDelim, 
                                              prec));
  return vectorContexts.size() - 1;
}

//! \param c Handle to the new context to select.
//! \return The handle to the old context, so that it can be restored if
//!   needed.
//! \warning If no context \c c exists, then nothing will happen.
KVectorContext Kalman::selectKVectorContext(KVectorContext c) {

  if (c >= vectorContexts.size()) {
    return currentVectorIndex;
  }

  KVectorContext tmp = currentVectorIndex;
  currentVectorIndex = c;
  currentVectorContext = &vectorContexts[c];
  return tmp;
}

//! Global pointer to the currently selected matrix printing context. Never 0.
KMatrixContextImpl* Kalman::currentMatrixContext = &matrixContexts[0];

//! Global index of the default matrix printing context.
KMatrixContext Kalman::DEFAULT_MATRIX_CONTEXT = 0;

//! This function creates a new matrix printing context and adds it to the
//! global list of matrix contexts. In the discussion below, a whitespace
//! refers to a single character among the following : a horizontal space, 
//! a horizontal tabulation, a linefeed or a carriage return.
//! \warning Severe constraints are imposed on delimiter strings. Pay
//!   attention to them, or else, weird bugs could happen !
//! \param elemDelim Delimiter string between matrix elements. Must not be
//!   empty. Must start and end with one or more whitespace(s). Cannot contain
//!   a middle whitespace (a lone whitespace which is not at the beginning, 
//!   nor at the end of the string).
//! \param rowDelim Delimiter string at the end of each row. Must not be
//!   empty. Must start and end with one or more whitespace(s). Cannot contain
//!   a middle whitespace (a lone whitespace which is not at the beginning, 
//!   nor at the end of the string).
//! \param startDelim Starting string before first matrix element. May be
//!   empty. If not empty, must not begin with a whitespace and must end
//!   with one or more whitespace(s). Cannot contain a middle whitespace.
//! \param endDelim Ending string after last matrix element. May be
//!   empty. If not empty, must not end with a whitespace and must begin
//!   with one or more whitespace(s). Cannot contain a middle whitespace
//! \param prec Number of significant digits to output. Must be between 1 and
//!   9, or else, will be clipped.
//! \return A handle to the newly created vector context.
KMatrixContext Kalman::createKMatrixContext(std::string elemDelim,
                                            std::string rowDelim,
                                            std::string startDelim,
                                            std::string endDelim,
                                            unsigned prec) {
  if (prec < 1) prec = 1;
  if (prec > 9) prec = 9;

  matrixContexts.push_back(KMatrixContextImpl(elemDelim, 
                                              rowDelim, 
                                              startDelim, 
                                              endDelim, 
                                              prec));
  return matrixContexts.size() - 1;
}

//! \param c Handle to the new context to select.
//! \return The handle to the old context, so that it can be restored if
//!   needed.
//! \warning If no context \c c exists, then nothing will happen.
KMatrixContext Kalman::selectKMatrixContext(KMatrixContext c) {

  if (c >= matrixContexts.size()) {
    return currentMatrixIndex;
  }

  KMatrixContext tmp = currentMatrixIndex;
  currentMatrixIndex = c;
  currentMatrixContext = &matrixContexts[c];
  return tmp;
}
