#include "MFile.h"
#include <iostream>
#include <fstream>
#include <string>

using namespace Kalman;

MFileElement::MFileElement()
{
  Index=0;
  Rows=0;
  Cols=0;
}

MFileElement::MFileElement(const MFileElement& tmp)
{
  Index = tmp.Index;
  Rows = tmp.Rows;
  Cols = tmp.Cols;
  Name = tmp.Name;
}

MFileElement::~MFileElement()
{
}

MFileElement& MFileElement::operator=(const MFileElement& tmp)
{
  Index = tmp.Index;
  Rows = tmp.Rows;
  Cols = tmp.Cols;
  Name = tmp.Name;
  return *this;
}


MFile::MFile()
{
}

MFile::~MFile()
{
}

int MFile::read(char * filename)
{
  std::ifstream file(filename, std::ios::in);
  char tmpline[LINE_MAX_LENGTH];
  std::string tmpstring;
  std::string tmpvalue;
  int a, b;
  unsigned int nbcol=0;
  char *tmpindex;
  MFileElement tmpElement;

  if(file)
    {
      file.getline(tmpline, LINE_MAX_LENGTH);
      
      while(!file.eof())
	{

	  tmpindex = strstr(tmpline, "%");

	  // Remove comment
	  if (tmpindex!=NULL)
	    tmpindex[0]=0;

	  tmpstring = tmpline;
	  
	  a = tmpstring.find("=",0);
	  
	  if (a!=std::string::npos)
	    {
	      // New element
	      tmpElement.Name = tmpstring.substr(0,a);

	      // Enlever les espaces inutiles
	      b = tmpElement.Name.find_last_of(" ");
	      if (b!=std::string::npos)
		{
		  tmpElement.Name.erase(b);
		}
	      
	      tmpElement.Rows = 1;
	      tmpElement.Cols = 0;
	      tmpElement.Index = Data.size();

	      tmpstring = tmpstring.substr(a+1, std::string::npos);
	    }

	  char current;
	  tmpvalue.erase(tmpvalue.begin(), tmpvalue.end());
	  for(unsigned int i=0; i<tmpstring.size(); i++)
	    {
	      current = tmpstring[i];
	      switch(current)
		{
		case '[':
		  tmpvalue.erase(tmpvalue.begin(), tmpvalue.end());
		  nbcol=0;
		  break;
		  
		case ']':
		  if (add_double(tmpvalue))
		    nbcol++;
		  
		  if (nbcol>tmpElement.Cols)
		    tmpElement.Cols=nbcol;

		  // Do not count incomplete row
		  if (nbcol<tmpElement.Cols)
		    tmpElement.Rows--;

		  VectorMFileElement.push_back(tmpElement);
		  break;

		case ';':
		  if (add_double(tmpvalue))
		    nbcol++;

		  if (nbcol>tmpElement.Cols)
		    tmpElement.Cols=nbcol;

		  nbcol=0;

		  tmpElement.Rows++;
		  break;

		case ' ':
		  if (add_double(tmpvalue))
		    nbcol++;
		  break;

		case '\t':
		  if (add_double(tmpvalue))
		    nbcol++;
		  break;

		default:
		  tmpvalue.append(1,current);
		  break;
		}
	      
	    }

	  file.getline(tmpline, LINE_MAX_LENGTH);
	  
	}
    }
  else
    {
      std::cerr << "MFile::read: Can not open file "<<filename<<std::endl;
      return -1;
    }
  return 0;
}

void MFile::print()
{
  for(unsigned int i=0; i<VectorMFileElement.size(); i++)
    {
      std::cout<<VectorMFileElement[i].Name.c_str()<<": Rows: "<<VectorMFileElement[i].Rows<<" Cols: "<<VectorMFileElement[i].Cols<<" Index: "<<VectorMFileElement[i].Index<<std::endl;
    }
}

bool MFile::add_double(std::string &tmpstr)
{
  double tmpdouble;

  if (!tmpstr.empty())
    {
      tmpdouble = atof(tmpstr.c_str());
      Data.push_back(tmpdouble);
      tmpstr.erase(tmpstr.begin(), tmpstr.end());
      return true;
    }
  return false;
}


int MFile::save(char *filename)
{
  std::ofstream file(filename, std::ios::out|std::ios::trunc);
  unsigned int index;

  if(file)
    {

      for( unsigned int i=0; i<VectorMFileElement.size(); i++)
	{
	  file<<VectorMFileElement[i].Name.c_str()<<" = ["<<std::endl;

	  index = VectorMFileElement[i].Index;
	  for( unsigned int j=0; j<VectorMFileElement[i].Rows; j++)
	    {
	      for( unsigned int k=0; k<VectorMFileElement[i].Cols; k++)
		{
		  file<<Data[index++]<<" ";
		}
	      file<<";"<<std::endl;
	    }

	  file<<"];"<<std::endl<<std::endl;
	}
    }
  else
    {
      std::cerr << "MFile::save: Can not open file "<<filename<<std::endl;
      return -1;
    }

  return 0;
}
