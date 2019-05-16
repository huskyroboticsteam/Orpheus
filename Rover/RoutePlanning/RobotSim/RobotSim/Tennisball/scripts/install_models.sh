#!/bin/bash
########################################################
### NOTE: THIS SCRIPT SHOULD BE RUN FROM THE PROJECT ###
###     DIRECTORY, **NOT** THE SCRIPTS DIRECTORY!    ###
########################################################
COCO_DIR="ssd_mobilenet_v1_coco_2018_01_28"
COCO_FILENAME="$COCO_DIR.tar.gz"
COCO_DOWNLOAD_URL="http://download.tensorflow.org/models/object_detection/$COCO_FILENAME"

dir=`pwd`
scripts_dir=$dir"/scripts"
cd /tmp

#clone the object detection models
git clone https://github.com/tensorflow/models
cd $dir

#copy the object detection models into working dir
rsync -ravP /tmp/models/research/object_detection ./

#compile protobufs
protoc object_detection/protos/*.proto --python_out=./object_detection/

#copy the modified model_main into object_detection dir
cp $scripts_dir/model_main.py ./object_detection/
DEST_DIR=$dir"/data/models"

#download the COCO MobileNet SSD V1 starter models and put them in data/models dir
mkdir -p $DEST_DIR
cd /tmp
wget $COCO_DOWNLOAD_URL -O $COCO_FILENAME
tar xvf $COCO_FILENAME
cp $COCO_DIR/model.ckpt.data-00000-of-00001 $DEST_DIR
cp $COCO_DIR/model.ckpt.index $DEST_DIR
cp $COCO_DIR/model.ckpt.meta $DEST_DIR
rm -rf $COCO_DIR
rm -rf $COCO_FILENAME
cd $dir
