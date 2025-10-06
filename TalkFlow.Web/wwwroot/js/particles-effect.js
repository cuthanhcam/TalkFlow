/* ========================================
   FLOATING PARTICLES BACKGROUND EFFECT
   ======================================== */

// Tạo hiệu ứng particles rơi
document.addEventListener('DOMContentLoaded', function() {
    createFloatingParticles();
});

function createFloatingParticles() {
    const canvas = document.createElement('canvas');
    canvas.id = 'particles-canvas';
    canvas.style.position = 'fixed';
    canvas.style.top = '0';
    canvas.style.left = '0';
    canvas.style.width = '100%';
    canvas.style.height = '100%';
    canvas.style.pointerEvents = 'none';
    canvas.style.zIndex = '-1';
    canvas.style.opacity = '0.6';
    
    document.body.appendChild(canvas);
    
    const ctx = canvas.getContext('2d');
    canvas.width = window.innerWidth;
    canvas.height = window.innerHeight;
    
    const particles = [];
    const particleCount = 50;
    
    // Màu sắc particles
    const colors = [
        'rgba(99, 102, 241, 0.6)',   // Primary indigo
        'rgba(6, 182, 212, 0.6)',    // Secondary cyan
        'rgba(245, 158, 11, 0.4)',   // Accent amber
        'rgba(129, 140, 248, 0.5)',  // Light indigo
        'rgba(34, 211, 238, 0.5)'    // Light cyan
    ];
    
    // Tạo particles
    for (let i = 0; i < particleCount; i++) {
        particles.push({
            x: Math.random() * canvas.width,
            y: Math.random() * canvas.height,
            size: Math.random() * 4 + 1,
            speedX: (Math.random() - 0.5) * 1,
            speedY: Math.random() * 2 + 1,
            color: colors[Math.floor(Math.random() * colors.length)],
            opacity: Math.random() * 0.8 + 0.2
        });
    }
    
    function animate() {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        
        particles.forEach(particle => {
            // Cập nhật vị trí
            particle.x += particle.speedX;
            particle.y += particle.speedY;
            
            // Reset particle khi ra khỏi màn hình
            if (particle.y > canvas.height) {
                particle.y = -10;
                particle.x = Math.random() * canvas.width;
            }
            if (particle.x > canvas.width) {
                particle.x = 0;
            }
            if (particle.x < 0) {
                particle.x = canvas.width;
            }
            
            // Vẽ particle
            ctx.beginPath();
            ctx.arc(particle.x, particle.y, particle.size, 0, Math.PI * 2);
            ctx.fillStyle = particle.color;
            ctx.globalAlpha = particle.opacity;
            ctx.fill();
            
            // Thêm hiệu ứng glow
            ctx.shadowColor = particle.color;
            ctx.shadowBlur = 10;
            ctx.fill();
            ctx.shadowBlur = 0;
        });
        
        ctx.globalAlpha = 1;
        requestAnimationFrame(animate);
    }
    
    animate();
    
    // Responsive resize
    window.addEventListener('resize', function() {
        canvas.width = window.innerWidth;
        canvas.height = window.innerHeight;
    });
}