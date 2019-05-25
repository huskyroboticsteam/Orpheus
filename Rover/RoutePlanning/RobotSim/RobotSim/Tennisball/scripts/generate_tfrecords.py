import csv, argparse, io, os
import tensorflow as tf
from PIL import Image
from object_detection.utils import dataset_util

csv_filename = "./data/training/training.csv"
image_directory = "./images/"
output_file = "./data/training/train.tfrecord"

def create_tf_example(row):
    filename = row['filename']
    bin_filename = row['filename'].encode('utf8')
    image_format = row['filename'][-3:].encode('utf8')

    image_file_path = os.path.join(image_directory, filename)
    with open(image_file_path, 'rb') as imagefile:
        encoded_image = imagefile.read()
    encoded_image_io = io.BytesIO(encoded_image)
    image = Image.open(encoded_image_io)
    width, height = image.size

    xmins = [(float(row['xmin']) / width)]
    xmaxs = [(float(row['xmax']) / width)]
    ymins = [(float(row['ymin']) / height)]
    ymaxs = [(float(row['ymax']) / height)]
    print("  Bounding box: [{}, {}] x [{}, {}], encoded: [{}, {}] x [{}, {}]"
          .format(row['xmin'], row['ymin'], row['xmax'], row['ymax'],
                  xmins, ymins, xmaxs, ymaxs))
    classes_text = [row['class'].encode('utf8')]
    classes = [1]

    tf_example = tf.train.Example(features=tf.train.Features(feature={
        'image/height': dataset_util.int64_feature(height),
        'image/width': dataset_util.int64_feature(width),
        'image/filename': dataset_util.bytes_feature(bin_filename),
        'image/source_id': dataset_util.bytes_feature(bin_filename),
        'image/encoded': dataset_util.bytes_feature(encoded_image),
        'image/format': dataset_util.bytes_feature(image_format),
        'image/object/bbox/xmin': dataset_util.float_list_feature(xmins),
        'image/object/bbox/xmax': dataset_util.float_list_feature(xmaxs),
        'image/object/bbox/ymin': dataset_util.float_list_feature(ymins),
        'image/object/bbox/ymax': dataset_util.float_list_feature(ymaxs),
        'image/object/class/text': dataset_util.bytes_list_feature(classes_text),
        'image/object/class/label': dataset_util.int64_list_feature(classes),
    }))
    return tf_example

if __name__ == '__main__':
    parser = argparse.ArgumentParser("Generates TFRecord files for a dataset.")
    parser.add_argument("-f","--file", nargs='?', const=csv_filename, default=csv_filename, help="The CSV file to use, if not the default one ("+csv_filename+")")
    parser.add_argument('-d',"--dir", nargs="?", const=image_directory, default=image_directory, help="The directory containing your images, if not the default one ("+image_directory+")")
    parser.add_argument('-o',"--output", nargs="?", const=output_file, default=output_file, help="The TFRecord file to output to, if not the default one ("+output_file+")")
    arg = parser.parse_args()
    csv_filename = arg.file
    image_directory = arg.dir
    output_file = arg.output

    writer = tf.python_io.TFRecordWriter(output_file)

    with open(csv_filename, 'r') as csvfile:
        csvreader = csv.DictReader(csvfile)
        i = 0
        for row in csvreader:
            print("Creating TFRecord #{} for image {}".format(row['index'],row['filename']))
            tfrecord = create_tf_example(row)
            writer.write(tfrecord.SerializeToString())
            i+=1

        writer.close()
        print("Successfully wrote {i} TFRecords to {output}".format(i=i,output=output_file))
