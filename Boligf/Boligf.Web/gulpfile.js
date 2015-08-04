var gulp = require('gulp');
var sass = require('gulp-sass');
var minifyCss = require('gulp-minify-css');
var rename = require("gulp-rename");
var ts = require('gulp-typescript');

gulp.task('sass', function () {
  gulp.src('./Styles/*.scss')
    .pipe(sass().on('error', sass.logError))
    .pipe(gulp.dest('./Styles'));
});

gulp.task('sass-compressed', function () {
  gulp.src('./Content/Styles/*.scss')
    .pipe(sass({outputStyle: 'compressed'}))
    .pipe(minifyCss({compatibility: ''}))
    .pipe(rename(function (path) {
      path.basename += ".min";
      path.extname = ".css";
    }))
    .pipe(gulp.dest('./Styles'));
});
 
var tsProject = ts.createProject({
	noImplicitAny: false,
	target: 'ES5',
	out: 'app.js'
});

gulp.task('typescript', function () {

	var tsResult = gulp.src(['Application/**/*.ts', 'Scripts/**/*.ts'])
                    .pipe(ts(tsProject));

	return tsResult.js.pipe(gulp.dest('Scripts'));
});


gulp.task('watch-sass', function() {
	gulp.watch('Styles/**/*.scss', [
		'sass',
		'sass-compressed'
	]);
});

gulp.task('watch-ts', function () {
	gulp.watch('Application/**/*.ts', ['typescript']);
});