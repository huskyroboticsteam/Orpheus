function exportMicrob(fid, nom, commentaire, matrice)

[x,y] = size(matrice);
fprintf(fid,'%s:MC_MATRIX {\n[%s]\n%d %d\n',nom, commentaire, x,y);
for i=1:x
    for j=1:y
         fprintf(fid,'%3.6f\t', matrice(i,j));
    end
    fprintf(fid,'\n');
end
fprintf(fid,'}\n\n');
