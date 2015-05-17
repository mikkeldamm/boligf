'use strict';

var gulp = require('gulp');
var sass = require('gulp-sass');
var typescript = require('gulp-typescript');

gulp.task('default', ['sass:watch', 'ts:watch']);

gulp.task('sass', function () {
  gulp.src('./Boligf/Boligf.Web/**/*.scss')
    .pipe(sass().on('error', sass.logError))
    .pipe(gulp.dest('./Boligf/Boligf.Web/Styles'));
});

gulp.task('sass:watch', function () {
  gulp.watch('./Boligf/Boligf.Web/**/*.scss', ['sass']);
});

gulp.task('ts', function () {
  var tsResult = gulp.src('./Boligf/Boligf.Web/**/*.ts')
    .pipe(typescript({
        noImplicitAny: true,
        out: 'app.js'
      }));
  return tsResult.js.pipe(gulp.dest('./Boligf/Boligf.Web/Scripts'));
});

gulp.task('ts:watch', function () {
  gulp.watch('./Boligf/Boligf.Web/**/*.ts', ['ts']);
});

/*
gulp.task('sass:watch', function () {
  gulp.watch('./s.scss', ['sass']);
});
*/
