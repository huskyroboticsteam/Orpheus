WINFILEPATH=$1
REMOTE=$2
FILEPATHDLL=$(echo $WINFILEPATH | sed -re 's/\\/\//g; s/([A-Z]):/\/mnt\/\L\1\E/i;')
FILEPATHEXE=$(echo $WINFILEPATH | sed -re 's/\\/\//g; s/([A-Z]):/\/mnt\/\L\1\E/i;')
FILEPATHDLL+="*.dll"
FILEPATHEXE+="*.exe"
echo $FILEPATHDLL
echo $FILEPATHEXE
echo $REMOTE
ssh-keygen -f ~/.ssh/known_hosts -R $REMOTE
# sshpass allows us to send the password directly inline without ssh keys setup
# -oStrictHostKeyChecking=no allows us to ignore whether the host key is in our hosts file
sshpass -p "temppwd" scp -oStrictHostKeyChecking=no $FILEPATHDLL $FILEPATHEXE debian@$REMOTE:~