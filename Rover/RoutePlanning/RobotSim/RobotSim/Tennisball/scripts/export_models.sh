#!/bin/bash
########################################################
### NOTE: THIS SCRIPT SHOULD BE RUN FROM THE PROJECT ###
###     DIRECTORY, **NOT** THE SCRIPTS DIRECTORY!    ###
########################################################

#Change this if your model name changes for any reason, like you do more training steps
MODEL_PATH_PREFIX="data/models/model.ckpt-50000"

#Change this if the path to pipeline.config ever changes
PIPELINE_PATH="training/pipeline.config"

#Change this to the desired temporary output path, relative to the project directory
EXPORT_PATH="data/models/export"

#Change this to the path where you would like frozen_inference_graph.pb
#  and graph.pbtxt to be finally placed.
FINAL_EXPORT_PATH="data/final_models/"

#Create directories if they don't exist
mkdir -p $EXPORT_PATH
mkdir -p $FINAL_EXPORT_PATH
#Export the inference graphs
python object_detection/export_inference_graph.py \
       --input_type image_tensor \
       --pipeline_config_path $PIPELINE_PATH \
       --trained_checkpoint_prefix $MODEL_PATH_PREFIX \
       --output_directory $EXPORT_PATH

#Generate .pbtxt for the graph
python scripts/tf_text_graph_ssd.py \
       --input $EXPORT_PATH/frozen_inference_graph.pb \
       --output $EXPORT_PATH/graph.pbtxt \
       --config $PIPELINE_PATH

#Copy the graph and graph.pbtxt to the final export path
cp $EXPORT_PATH/frozen_inference_graph.pb $FINAL_EXPORT_PATH/
cp $EXPORT_PATH/graph.pbtxt $FINAL_EXPORT_PATH/

#Delete the temporary export path
rm -r $EXPORT_PATH
