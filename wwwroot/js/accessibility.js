document.addEventListener('DOMContentLoaded', function () {

    const html = document.documentElement;

    const KEY_FONT = 'gcms_fontsize';
    const KEY_CONTRAST = 'gcms_contrast';
    const KEY_SPACING = 'gcms_textspacing';

    const fontDecrease = document.getElementById('fontDecrease');
    const fontReset = document.getElementById('fontReset');
    const fontIncrease = document.getElementById('fontIncrease');
    const contrastToggle = document.getElementById('contrastToggle');
    const spacingToggle = document.getElementById('spacingToggle');

    const MIN_LEVEL = 1;
    const MAX_LEVEL = 6;

    // ---------------- Font size ----------------
    let fontLevel = parseInt(localStorage.getItem(KEY_FONT), 10);
    if (!fontLevel || fontLevel < MIN_LEVEL || fontLevel > MAX_LEVEL) fontLevel = 1;

    function applyFontLevel() {
        html.setAttribute('data-fontsize', fontLevel);
        localStorage.setItem(KEY_FONT, fontLevel);
        if (fontIncrease) fontIncrease.disabled = fontLevel >= MAX_LEVEL;
        if (fontDecrease) fontDecrease.disabled = fontLevel <= MIN_LEVEL;
    }
    applyFontLevel();

    if (fontIncrease) {
        fontIncrease.addEventListener('click', () => {
            fontLevel = Math.min(MAX_LEVEL, fontLevel + 1);
            applyFontLevel();
        });
    }
    if (fontDecrease) {
        fontDecrease.addEventListener('click', () => {
            fontLevel = Math.max(MIN_LEVEL, fontLevel - 1);
            applyFontLevel();
        });
    }
    if (fontReset) {
        fontReset.addEventListener('click', () => {
            fontLevel = 1;
            applyFontLevel();
        });
    }

    // ---------------- High contrast ----------------
    function setContrast(on) {
        html.classList.toggle('high-contrast', on);
        if (contrastToggle) contrastToggle.setAttribute('aria-pressed', on ? 'true' : 'false');
        localStorage.setItem(KEY_CONTRAST, on ? '1' : '0');
    }
    setContrast(localStorage.getItem(KEY_CONTRAST) === '1');
    if (contrastToggle) {
        contrastToggle.addEventListener('click', () => {
            setContrast(!html.classList.contains('high-contrast'));
        });
    }

    // ---------------- Text spacing (WCAG 1.4.12) ----------------
    function setSpacing(on) {
        html.classList.toggle('text-spacing-mode', on);
        if (spacingToggle) spacingToggle.setAttribute('aria-pressed', on ? 'true' : 'false');
        localStorage.setItem(KEY_SPACING, on ? '1' : '0');
    }
    setSpacing(localStorage.getItem(KEY_SPACING) === '1');
    if (spacingToggle) {
        spacingToggle.addEventListener('click', () => {
            setSpacing(!html.classList.contains('text-spacing-mode'));
        });
    }

});
