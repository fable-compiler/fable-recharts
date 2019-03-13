module.exports = {
  entry: resolve('Fable.Recharts.Sample.fsproj'),
  output: {
    filename: 'bundle.js',
    path: resolve('.'),
  },
  devServer: {
    contentBase: resolve('.'),
    port: 8080
  },
  module: {
    rules: [
      {
        test: /\.fs(x|proj)?$/,
        use: "fable-loader"
      }
    ]
  }
};

function resolve(filePath) {
    return require("path").join(__dirname, filePath)
}
