function exportMatlab(fid, name, matrix)

[x,y] = size(matrix);
fprintf(fid,'%s = [\n',name);
for i=1:x
    for j=1:y
         fprintf(fid,'%f\t', matrix(i,j));
    end
    fprintf(fid,';\n');
end
fprintf(fid,'];\n\n');
