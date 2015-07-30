var gulp = require('gulp');
var sass = require('gulp-sass');
var minifyCss = require('gulp-minify-css');
var rename = require("gulp-rename");

gulp.task('sass', function () {
  gulp.src('./Styles/*.scss')
    .pipe(sass().on('error', sass.logError))
    .pipe(gulp.dest('./Styles'));
});

gulp.task('sass:compressed', function () {
  gulp.src('./Content/Styles/*.scss')
    .pipe(sass({outputStyle: 'compressed'}))
    .pipe(minifyCss({compatibility: ''}))
    .pipe(rename(function (path) {
      path.basename += ".min";
      path.extname = ".css";
    }))
    .pipe(gulp.dest('./Styles'));
});
 
gulp.task('sass:watch', function () {
  gulp.watch('Styles/**/*.scss', [
      'sass', 
      'sass:compressed'
  ]);
});
