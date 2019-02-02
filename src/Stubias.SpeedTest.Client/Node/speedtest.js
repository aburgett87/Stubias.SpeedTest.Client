module.exports.test = function(cb) {
  var speedTest = require('speedtest-net');
  var test = speedTest({pingCount: 100});

  test.on('error', err => {
    console.error(err);
  });

  test.on('data', data  => {
    cb(null, data);
  });
}