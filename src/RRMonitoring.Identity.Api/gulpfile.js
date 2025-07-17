'use strict';

const path = {
    build: {
        ts: 'assets/js/',
        css: 'assets/css/',
        img: 'assets/images/',
        fonts: 'assets/fonts/',
    },
    src: {
        ts: 'src/ts/main.ts',
        style: 'src/styles/main.scss',
        img: 'src/images/**/*.*',
        fonts: 'src/fonts/**/*.*',
    },
    watch: {
        ts: 'src/ts/**/*.ts',
        style: 'src/styles/**/*.scss',
        img: 'src/images/**/*.*',
        fonts: 'src/fonts/**/*.*',
    },
    clean: './assets/*'
};

const gulp = require('gulp'),
    plumber = require('gulp-plumber'),
    rigger = require('gulp-rigger'),
    sourcemaps = require('gulp-sourcemaps'),
    sass = require('gulp-sass')(require('sass')),
    autoprefixer = require('gulp-autoprefixer'),
    cleanCSS = require('gulp-clean-css'),
    uglify = require('gulp-uglify-es').default,
    cache = require('gulp-cache'),
    rimraf = require('gulp-rimraf'),
    rename = require('gulp-rename'),
    ts = require('gulp-typescript');

gulp.task('css:build', function () {
    return gulp.src(path.src.style)
        .pipe(plumber())
        .pipe(sourcemaps.init())
        .pipe(sass().on('error', sass.logError))
        .pipe(autoprefixer())
        .pipe(gulp.dest(path.build.css))
        .pipe(rename({ suffix: '.min' }))
        .pipe(cleanCSS())
        .pipe(sourcemaps.write('./'))
        .pipe(gulp.dest(path.build.css))
});

gulp.task('ts:build', function () {
    return gulp.src(path.src.ts)
        .pipe(ts({
            noImplicitAny: true,
            outFile: 'main.js'
        }))
        .pipe(gulp.dest(path.build.ts))
        .pipe(rename({ suffix: '.min' }))
        .pipe(sourcemaps.init())
        .pipe(uglify())
        .pipe(sourcemaps.write('./'))
        .pipe(gulp.dest(path.build.ts));
});

gulp.task('image:build', function () {
    return gulp.src(path.src.img)
        .pipe(gulp.dest(path.build.img));
});

gulp.task('fonts:build', function () {
    return gulp.src(path.src.fonts)
        .pipe(gulp.dest(path.build.fonts));
});

gulp.task('clean:build', function () {
    return gulp.src(path.clean, { read: false })
        .pipe(rimraf());
});

gulp.task('cache:clear', function () {
    cache.clearAll();
});

gulp.task('build', gulp.series('clean:build', 'css:build', 'ts:build', 'image:build', 'fonts:build'));

gulp.task('watch', function () {
    gulp.watch(path.watch.style, gulp.parallel('css:build'));
    gulp.watch(path.watch.ts, gulp.parallel('ts:build'));
    gulp.watch(path.watch.img, gulp.parallel('image:build'));
    gulp.watch(path.watch.fonts, gulp.parallel('fonts:build'));
});

gulp.task('default', gulp.parallel('build', 'watch'));
