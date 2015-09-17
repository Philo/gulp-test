/// <vs BeforeBuild='beforebuild' />
var gulp = require("gulp");
var config = require("./gulp-config.json");
var concat = require("gulp-concat");
var argv = require("yargs").argv;
var gulpif = require("gulp-if");

var sass = require("gulp-sass");

var minifyCss = require("gulp-minify-css");
var autoprefixer = require("gulp-autoprefixer");

var uglify = require("gulp-uglify");
var jshint = require("gulp-jshint");

var imagemin = require("gulp-imagemin");
var changed = require("gulp-changed");

var rename = require('gulp-rename');

var del = require('del');
var sequence = require('run-sequence');

gulp.task('clean', function() {
    return del(config.outputPath.dist);
});

gulp.task("appSass", function () {
    return gulp.src(config.appSassPath)
        .pipe(sass({ errLogToConsole: true }))
        .pipe(autoprefixer({
            browsers: ["last 2 versions"],
            cascade: true,
            remove: true
        }))
        .pipe(gulpif(argv.production, minifyCss()))
        .pipe(gulp.dest(config.outputPath.appSass));
});

gulp.task("appJs", function () {
    return gulp.src(config.appJsPath)
        .pipe(jshint())
        .pipe(jshint.reporter("default"))
        .pipe(gulpif(argv.production, uglify()))
        .pipe(gulp.dest(config.outputPath.appJs));
});

gulp.task("images", function () {
    return gulp.src(config.images)
        .pipe(changed(config.outputPath.images, { hasChanged: changed.compareSha1Digest }))
        .pipe(gulpif(!argv.production, imagemin({
            progressive: true,
            optimizationLevel: 3
        })))
        .pipe(gulp.dest(config.outputPath.images));
});

gulp.task('extras', function () {
    return gulp.src(config.extras).pipe(gulp.dest(config.outputPath.dist));
});

gulp.task("default", function(cb) {    
    sequence("clean", ["appSass", "appJs", "images", "extras"], cb);
});

gulp.task("watch", function () {
    gulp.watch(config.appSassPath, ["appSass"]);
    gulp.watch(config.appJsPath, ["appJs"]);
});

gulp.task("beforebuild", ["appSass", "appJs"]);