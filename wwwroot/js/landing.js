const nav = document.getElementById('nav');
const burger = document.getElementById('burger');
const links = document.getElementById('links');
addEventListener('scroll', () => nav.classList.toggle('scrolled', scrollY > 30), { passive: true });
burger.addEventListener('click', () => {
  const open = links.classList.toggle('open');
  burger.setAttribute('aria-expanded', open);
});
links.querySelectorAll('a').forEach(a => a.addEventListener('click', () => {
  links.classList.remove('open'); burger.setAttribute('aria-expanded', false);
}));
