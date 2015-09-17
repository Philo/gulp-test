/// <vs SolutionOpened='sf' />
// = Packages
//-----------------------------------------------------------------------------//

    var gulp            = require('gulp'),
        autoprefixer    = require('gulp-autoprefixer'),
        cache           = require('gulp-cache'),
        imagemin        = require('gulp-imagemin'),
        plumber         = require('gulp-plumber'),
        sass            = require('gulp-sass'),
        shell           = require('gulp-shell'),
        runSequence     = require('run-sequence'),
        del             = require('del');


// = Global vars
//-----------------------------------------------------------------------------//

    //     assets          = useref.assets();

    var onError = function (err) {
        //gutil.beep();
        console.log(err);
        this.emit('end');
    };

// = default
//-----------------------------------------------------------------------------//
    gulp.task('default', function() {
        runSequence('clean', ['clean', 'sass', 'images', 'scripts', 'extras']);
    });


// = Clean
//-----------------------------------------------------------------------------//

    gulp.task('clean', function() {
        return del('_/dist');
    });
    

// = Sass
//-----------------------------------------------------------------------------//

    gulp.task('sass', function() {
        return gulp.src('_/src/sass/**/*.scss')
        .pipe(plumber(onError))
        .pipe(sass())
        .pipe(autoprefixer({
            browsers: ['last 2 versions'],
            cascade: false
        }))
        .pipe(gulp.dest('_/dist/css/'));
    });


// = Images
//-----------------------------------------------------------------------------//

    gulp.task('images', function() {
        return gulp.src('_/src/images/**/*')
        .pipe(cache(imagemin({
            progressive: true,
            interlaced: true, 
            svgoPlugins: [{cleanupIDs: false}]
        })))
        .pipe(gulp.dest('_/dist/images/'));
    });


// = Javascript
//-----------------------------------------------------------------------------//

    gulp.task('scripts', function() {
        return gulp.src('_/src/js/**/*')
        .pipe(gulp.dest('_/dist/js/'))
    });


// = Extras
//-----------------------------------------------------------------------------//

    gulp.task('extras', function () {
        return gulp.src(['_/src/**/*', '!_/src/{sass,sass/**}', '!_/src/{images,images/**}', '!_/src/{js,js/**}'])
        .pipe(gulp.dest('_/dist/'));
    });


// = Serve
//-----------------------------------------------------------------------------//

    gulp.task('serve', ['sass', 'images', 'scripts', 'extras'], function() {
        var fs = require("fs");
        var browserSync = require('browser-sync');
        var reload = browserSync.reload;

        var fileContent=fs.readFileSync("gulp.config", "utf8");
        var config = JSON.parse(fileContent);
        browserSync({
            proxy: 'http://nss.local/',
            notify: false,
            host: config.external_ip
        });

        gulp.watch('_/src/sass/**/*.scss', ['sass', browserSync.reload]);

        gulp.watch([
            'Views/**/*.cshtml',
            '_/src/js/**/*'
        ]).on('change', browserSync.reload);

    });