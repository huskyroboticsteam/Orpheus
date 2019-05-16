#ifndef MFILE_IMPL_HPP
#define MFILE_IMPL_HPP

#include <string>

namespace Kalman {

template<typename T, K_UINT_32 BEG, bool DBG>
inline int MFile::get(std::string name, KVector<T,BEG,DBG>& tmpvector)
{
  unsigned int i;

  for(i=0; i<VectorMFileElement.size(); i++)
    {
      if (VectorMFileElement[i].Name == name)
	break;
    }

  if (i==VectorMFileElement.size())
    return -1;

  if (VectorMFileElement[i].Cols>VectorMFileElement[i].Rows)
    {
      // Row vector
      tmpvector.resize(VectorMFileElement[i].Cols);
      for(unsigned int j=0; j<VectorMFileElement[i].Cols; j++)
	{
	  tmpvector(BEG+j)=Data[VectorMFileElement[i].Index+j];
	}
    }
  else
    {
      // Column vector
      tmpvector.resize(VectorMFileElement[i].Rows);
      for(unsigned int j=0; j<VectorMFileElement[i].Rows; j++)
	{
	  tmpvector(BEG+j)=Data[VectorMFileElement[i].Index+j*VectorMFileElement[i].Cols];
	}
    }
  
  return 0;
}


template<typename T, K_UINT_32 BEG, bool DBG>
inline int MFile::get(std::string name, KMatrix<T,BEG,DBG>& tmpmatrix)
{
  unsigned int i;

  for(i=0; i<VectorMFileElement.size(); i++)
    {
      if (VectorMFileElement[i].Name == name)
	break;
    }

  if (i==VectorMFileElement.size())
    return -1;

  tmpmatrix.resize(VectorMFileElement[i].Rows, VectorMFileElement[i].Cols);

  for(unsigned int j=0; j<VectorMFileElement[i].Rows; j++)
    {
      for(unsigned int k=0; k<VectorMFileElement[i].Cols; k++)
	{
	  tmpmatrix(BEG+j,BEG+k) = Data[VectorMFileElement[i].Index + j*VectorMFileElement[i].Cols + k];
	}
    }
  
  return 0;
}


template<typename T, K_UINT_32 BEG, bool DBG>
inline int MFile::add(std::string name, KVector<T,BEG,DBG>& tmpvector, int type)
{
  unsigned int i;
  MFileElement tmpElement;
  double tmpdouble;

  for(i=0; i<VectorMFileElement.size(); i++)
    {
      if (VectorMFileElement[i].Name == name)
	break;
    }

  //Element exist!
  if (i!=VectorMFileElement.size())
    return -1;

  if (type == ROW_VECTOR)
    {
      tmpElement.Rows = 1;
      tmpElement.Cols = tmpvector.size();
      tmpElement.Name = name;
      tmpElement.Index = Data.size();
    }
  else
    {
      tmpElement.Rows = tmpvector.size();
      tmpElement.Cols = 1;
      tmpElement.Name = name;
      tmpElement.Index = Data.size();
    }

  VectorMFileElement.push_back(tmpElement);

  
  for(unsigned int i=0; i<tmpvector.size(); i++)
    {
      tmpdouble = tmpvector(BEG + i);
      Data.push_back(tmpdouble);
    }

  return 0;
}


template<typename T, K_UINT_32 BEG, bool DBG>
inline int MFile::add(std::string name, KMatrix<T,BEG,DBG>& tmpmatrix)
{
  unsigned int i;
  MFileElement tmpElement;
  double tmpdouble;


  for(i=0; i<VectorMFileElement.size(); i++)
    {
      if (VectorMFileElement[i].Name == name)
	break;
    }

  //Element exist!
  if (i!=VectorMFileElement.size())
    return -1;

  tmpElement.Rows = tmpmatrix.nrow();
  tmpElement.Cols = tmpmatrix.ncol();
  tmpElement.Name = name;
  tmpElement.Index = Data.size();
  VectorMFileElement.push_back(tmpElement);

  for(unsigned int i=0; i<tmpmatrix.nrow(); i++)
    {
      for(unsigned int j=0; j<tmpmatrix.ncol(); j++)
	{
	  tmpdouble = tmpmatrix(BEG + i,BEG + j);
	  Data.push_back(tmpdouble);
	}
    }

  return 0;
}

}

#endif
