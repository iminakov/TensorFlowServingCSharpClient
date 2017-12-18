import os
import sys
import tensorflow as tf
from tensorflow.examples.tutorials.mnist import input_data

tf.app.flags.DEFINE_integer('training_iteration', 1000,
                            'number of training iterations.')
tf.app.flags.DEFINE_integer('model_version', 1, 'version number of the model.')
tf.app.flags.DEFINE_string('work_dir', '/tmp', 'Working directory.')
FLAGS = tf.app.flags.FLAGS

def main(_):

  mnist = input_data.read_data_sets('MNIST_data', one_hot=True)

  def weight_variable(shape):
      initial = tf.truncated_normal(shape, stddev=0.1)
      return tf.Variable(initial)

  def bias_variable(shape):
      initial = tf.constant(0.1, shape=shape)
      return tf.Variable(initial)

  def conv2d(x, W):
      return tf.nn.conv2d(x, W, strides=[1, 1, 1, 1], padding='SAME')

  def max_pool_2x2(x):
      return tf.nn.max_pool(x, ksize=[1, 2, 2, 1], strides=[1, 2, 2, 1], padding='SAME')

  x = tf.placeholder(tf.float32, shape=[None, 784], name='x')
  y_ = tf.placeholder(tf.float32, shape=[None, 10])

  W = tf.Variable(tf.zeros([784,10]))
  b = tf.Variable(tf.zeros([10]))

  y = tf.matmul(x,W) + b

  #First Conv Layer
  W_conv1 = weight_variable([5, 5, 1, 32])
  b_conv1 = bias_variable([32])

  x_image = tf.reshape(x, [-1, 28, 28, 1])

  h_conv1 = tf.nn.relu(conv2d(x_image, W_conv1) + b_conv1)
  h_pool1 = max_pool_2x2(h_conv1)


  #Second Conv Layer
  W_conv2 = weight_variable([5, 5, 32, 64])
  b_conv2 = bias_variable([64])
  
  h_conv2 = tf.nn.relu(conv2d(h_pool1, W_conv2) + b_conv2)
  h_pool2 = max_pool_2x2(h_conv2)
  
  #Conn Layer
  W_fc1 = weight_variable([7 * 7 * 64, 1024])
  b_fc1 = bias_variable([1024])
  
  h_pool2_flat = tf.reshape(h_pool2, [-1, 7*7*64])
  h_fc1 = tf.nn.relu(tf.matmul(h_pool2_flat, W_fc1) + b_fc1)
  
  #DropOut
  keep_prob = tf.placeholder(tf.float32, name="keep_prob_training")
  h_fc1_drop = tf.nn.dropout(h_fc1, keep_prob)
  
  #Readout Layer
  W_fc2 = weight_variable([1024, 10])
  b_fc2 = bias_variable([10])
  
  y_conv = tf.matmul(h_fc1_drop, W_fc2) + b_fc2
  
  y_softmax = tf.nn.softmax(logits=y_conv, name='y')
  cross_entropy = tf.reduce_mean(-tf.reduce_sum(y_ * tf.log(y_softmax), [1]))
  train_step = tf.train.AdamOptimizer(1e-4).minimize(cross_entropy)

  values, indices = tf.nn.top_k(y_softmax, 10)
  table = tf.contrib.lookup.index_to_string_table_from_tensor(tf.constant([str(i) for i in range(10)]))
  prediction_classes = table.lookup(tf.to_int64(indices))

  correct_prediction = tf.equal(tf.argmax(y_conv, 1), tf.argmax(y_, 1))
  accuracy = tf.reduce_mean(tf.cast(correct_prediction, tf.float32))
  
  sess = tf.InteractiveSession()
  sess.run(tf.global_variables_initializer())
  for i in range(FLAGS.training_iteration):
    batch = mnist.train.next_batch(50)
    if i % 100 == 0:
      train_accuracy = accuracy.eval(feed_dict={x: batch[0], y_: batch[1], keep_prob: 1.0})
      print('step %d, training accuracy %g' % (i, train_accuracy))
    train_step.run(feed_dict={x: batch[0], y_: batch[1], keep_prob: 0.5})

  export_path_base = sys.argv[-1]
  print ('Try continue here !!! ', export_path_base)
  export_path = os.path.join(
      tf.compat.as_bytes(export_path_base),
      tf.compat.as_bytes(str(FLAGS.model_version)))
  print ('Exporting trained model to', export_path)
  builder = tf.saved_model.builder.SavedModelBuilder(export_path)

  tensor_info_x = tf.saved_model.utils.build_tensor_info(x)
  tensor_info_y = tf.saved_model.utils.build_tensor_info(y_softmax)
  tensor_info_keep_prob = tf.saved_model.utils.build_tensor_info(keep_prob)
  prediction_signature = (
      tf.saved_model.signature_def_utils.build_signature_def(
          inputs={'images': tensor_info_x, 'keep_prob': tensor_info_keep_prob},
          outputs={'scores': tensor_info_y},
          method_name=tf.saved_model.signature_constants.PREDICT_METHOD_NAME))
  legacy_init_op = tf.group(tf.tables_initializer(), name='legacy_init_op')
  builder.add_meta_graph_and_variables(
      sess, [tf.saved_model.tag_constants.SERVING],
      signature_def_map={
          'predict_images':
              prediction_signature,
      },
      legacy_init_op=legacy_init_op)
  builder.save()
  print ('Done exporting!')


if __name__ == '__main__':
  tf.app.run()