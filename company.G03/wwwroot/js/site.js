document.querySelector('form').addEventListener('submit', function (e) {
    const inputs = this.querySelectorAll('input[required]');
    let isValid = true;

    inputs.forEach(input => {
        if (!input.value.trim()) {
            input.nextElementSibling.style.display = 'block';
            isValid = false;
        } else {
            input.nextElementSibling.style.display = 'none';
        }
    });

    if (!isValid) {
        e.preventDefault(); // يمنع إرسال الفورم إذا كان فيه أخطاء
    }
});
@section Scripts {
    @if (ViewData["SuccessMessage"] != null) {
        <script>
            document.addEventListener('DOMContentLoaded', function() {
                // إعادة تعيين جميع حقول الإدخال
                document.querySelector('form').reset();

                // أو يمكنك إفراغ كل حقل يدوياً إذا أردت
                /*
                document.getElementById('UserName').value = '';
                document.getElementById('Email').value = '';
                // ... وهكذا لباقي الحقول
                */
            });
        </script>
    }
}
// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
<script>
    // إغلاق تلقائي بعد 5 ثوان
    setTimeout(() => {
        const alerts = document.querySelectorAll('.alert');
        alerts.forEach(alert => {
        alert.style.animation = 'fadeOut 0.5s forwards';
            setTimeout(() => alert.remove(), 500);
        });
    }, 5000);

    // تعريف fadeOut للأنيميشن
    document.head.insertAdjacentHTML('beforeend', `
    <style>
        @keyframes fadeOut {
            to {opacity: 0; transform: translate(-50%, -20px); }
        }
    </style>
    `);
</script>

particle_no = 25;

window.requestAnimFrame = (function () {
    return window.requestAnimationFrame ||
        window.webkitRequestAnimationFrame ||
        window.mozRequestAnimationFrame ||
        window.oRequestAnimationFrame ||
        window.msRequestAnimationFrame ||
        function (callback) {
            window.setTimeout(callback, 1000 / 60);
        };
})();

var canvas = document.getElementsByTagName("canvas")[0];
var ctx = canvas.getContext("2d");

var counter = 0;
var particles = [];
var w = 400,
    h = 200;
canvas.width = w;
canvas.height = h;

function reset() {
    // إما ترك الخلفية شفافة (لا ترسم أي شيء)
    // أو استخدام نفس لون CSS
    ctx.fillStyle = "#FFFFF0"; // نفس لون الخلفية
    ctx.fillRect(0, 0, w, h);


    ctx.fillStyle = "#D8D8D8";
    ctx.fillRect(25, 80, 350, 25);
}

function progressbar() {
    this.widths = 0;
    this.progress = 0;

    this.draw = function () {
        var currentColor = getGradientColor(this.progress);
        ctx.fillStyle = currentColor;
        ctx.fillRect(25, 80, this.widths, 25);

        var grad = ctx.createLinearGradient(0, 0, 0, 130);
        grad.addColorStop(0, "transparent");
        grad.addColorStop(1, "rgba(0,0,0,0.5)");
        ctx.fillStyle = grad;
        ctx.fillRect(25, 80, this.widths, 25);
    }
}

function getGradientColor(progress) {
    // اللون البداية: #FFFFFF (أبيض)
    var start = { r: 255, g: 255, b: 255 };
    // اللون النهاية: #0B2161 (كحلي داكن)
    var end = { r: 11, g: 33, b: 97 };

    // حساب اللون الحالي
    var r = Math.round(start.r + (end.r - start.r) * progress);
    var g = Math.round(start.g + (end.g - start.g) * progress);
    var b = Math.round(start.b + (end.b - start.b) * progress);

    return `rgb(${r},${g},${b})`;
}

function particle() {
    this.x = 23 + bar.widths;
    this.y = 82;
    this.vx = 0.8 + Math.random() * 1;
    this.v = Math.random() * 5;
    this.g = 1 + Math.random() * 3;
    this.down = false;

    this.draw = function () {
        var currentColor = getGradientColor(bar.progress);
        ctx.fillStyle = currentColor;
        var size = Math.random() * 2;
        ctx.fillRect(this.x, this.y, size, size);
    }
}

var bar = new progressbar();


function draw() {
    reset();
    counter++;

    bar.progress = Math.min(bar.widths / 350, 1);

    bar.widths += 2;
    if (bar.widths > 350) {
        if (counter > 215) {
            // الانتقال إلى الصفحة الرئيسية بعد التأخير
            setTimeout(function () {
                window.location.href = "Index.html"; // تغيير هذا المسار حسب احتياجك
            }, 1000); // تأخير 1 ثانية قبل الانتقال

            reset();
            bar.progress = 0;
            bar.widths = 0;
            counter = 0;
            particles = [];
        } else {
            bar.progress = 1;
            bar.widths = 351;
            bar.draw();
        }
    } else {
        bar.draw();
        if (counter % 3 === 0) {
            particles.push(new particle());
        }
    }
    update();
}


function update() {
    for (var i = 0; i < particles.length; i++) {
        var p = particles[i];
        p.x -= p.vx;
        if (p.down) {
            p.g += 0.1;
            p.y += p.g;
        } else {
            if (p.g < 0) {
                p.down = true;
                p.g += 0.1;
                p.y += p.g;
            } else {
                p.y -= p.g;
                p.g -= 0.1;
            }
        }
        p.draw();

        // إزالة الجسيمات التي خرجت عن النطاق
        if (p.x < 0 || p.y > h) {
            particles.splice(i, 1);
            i--;
        }
    }
}

function animloop() {
    draw();
    requestAnimFrame(animloop);
}

animloop();