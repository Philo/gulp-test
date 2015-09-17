/// <vs BeforeBuild='beforebuild' />
var gulp = require("gulp");
var config = require("./gulp-config.json");
var concat = require("gulp-concat");
var argv = require("yargs").argv;
var gulpif = require("gulp-if");

var bower = require("gulp-bower");

var sass = require("gulp-sass");
var scsslint = require("gulp-scss-lint");
var minifyCss = require("gulp-minify-css");
var autoprefixer = require("gulp-autoprefixer");

var replace = require("gulp-replace");

var uglify = require("gulp-uglify");
var jshint = require("gulp-jshint");

var imagemin = require("gulp-imagemin");
var changed = require("gulp-changed");

// var awspublish = require('gulp-awspublish');
var rename = require('gulp-rename');
var parallelize = require('concurrent-transform');

// require("./Areas/Admin/Content/gulpfile.js");

gulp.task("bower", function () {
    // return bower().pipe(gulp.dest(config.bowerDir));
});

gulp.task("appSass", function () {
    return gulp.src(config.appSassPath)
        // .pipe(gulpif(!argv.production, scsslint({
        //     'config': ".scss-lint.yml",
        //     'maxBuffer': 1048576
        // })))
        .pipe(sass({ errLogToConsole: true }))
        .pipe(concat("custom.css"))
        .pipe(autoprefixer({
            browsers: ["last 8 versions"],
            cascade: true,
            remove: true
        }))
        .pipe(gulpif(argv.production, minifyCss()))
        .pipe(gulp.dest(config.outputPath));
});

gulp.task("vendorCss", ["bower"], function () {
    return gulp.src(config.vendorCss.map(function (x) { return config.bowerDir + "/" + x; }))
        .pipe(replace("../fonts/", "./fonts/"))
        .pipe(replace("font/", "./fonts/"))
        .pipe(replace("sourceMappingURL", ""))
        .pipe(concat("styles.css"))
        .pipe(minifyCss())
        .pipe(gulp.dest(config.outputPath));
});

gulp.task("vendorJs", ["bower"], function () {
    return gulp.src(config.vendorJs.map(function (x) { return config.bowerDir + "/" + x; }))
        .pipe(concat("vendor.js"))
        .pipe(gulpif(argv.production, uglify()))
        .pipe(gulp.dest(config.outputPath));
});

gulp.task("appJs", function () {
    return gulp.src(config.appJsPath)
        .pipe(jshint())
        .pipe(jshint.reporter("default"))
        .pipe(concat("app.js"))
        .pipe(gulpif(argv.production, uglify()))
        .pipe(gulp.dest(config.outputPath));
});

gulp.task('fonts', ['bower'], function () {
    return gulp.src(config.fonts.map(function (x) {
        return x.indexOf("./") === 0 ? x : config.bowerDir + "/" + x;
    }))
    .pipe(gulp.dest(config.outputPath + "/fonts/"));
});

gulp.task("images", function () {
    return gulp.src(config.images)
        .pipe(changed(config.outputPath + "/images/", { hasChanged: changed.compareSha1Digest }))
        .pipe(gulpif(!argv.production, imagemin({
            progressive: true,
            optimizationLevel: 3
        })))
        //.pipe(gulp.dest("./Content/Images/"))
        .pipe(gulp.dest(config.outputPath + "/images/"));
});

gulp.task("default", ["bower", "appSass", "vendorCss", "vendorJs", "appJs", "fonts", "images"]);

gulp.task("watch", function () {
    gulp.watch(config.appSassPath, ["appSass"]);
    gulp.watch(config.appJsPath, ["appJs"]);
});

gulp.task("beforebuild", ["appSass", "appJs"]);

gulp.task('prepAssets', ['appSass'], function () {
    // return gulp.src(config.outputPath + "custom.css")
    //     .pipe(replace("/Content/Images/", config.awsBucketUrl + "images/"))
    //     .pipe(replace("/Content/Fonts/", config.awsBucketUrl + "fonts/"))
    //     .pipe(gulp.dest(config.outputPath));
});

gulp.task('publishAssets', ['default', 'prepAssets'], function () {
    // var publisher = awspublish.create({
    //     params: {
    //         Bucket: config.awsBucket
    //     },
    //     accessKeyId: config.awsAccessKey,
    //     secretAccessKey: config.awsSecretKey,
    //     region: 'us-east-1'
    // });

    // var src = [config.outputPath + "*.*", config.outputPath + "**/*.*", config.outputPath + "**/**/*.*"];

    // return gulp.src(src)
    //     .pipe(rename(function (path) {
    //         path.dirname = '/' + config.awsFolder + '/' + path.dirname.toLowerCase();
    //         path.basename = path.basename.toLowerCase();
    //     }))
    //     .pipe(awspublish.gzip())
    //     .pipe(parallelize(publisher.publish({ 'Cache-Control': 'max-age=31536000, public' }, { force: true }), 20))
    //     .pipe(awspublish.reporter());
});