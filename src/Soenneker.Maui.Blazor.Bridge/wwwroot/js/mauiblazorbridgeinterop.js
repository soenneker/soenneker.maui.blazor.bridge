export function observeElementPosition(element, id) {
    if (!element || !id)
        return;

    let timeoutId = null;

    const reportPosition = () => {
        const rect = element.getBoundingClientRect();

        var elementPositionDto = {
            top: rect.top,
            left: rect.left,
            width: rect.width,
            height: rect.height,
            viewportHeight: window.innerHeight
        };

        window.CallbackRegistryInterop.sendToCallback(id, elementPositionDto);
    };

    const debouncedReport = () => {
        if (timeoutId) {
            clearTimeout(timeoutId);
        }
        timeoutId = setTimeout(() => {
            reportPosition();
            timeoutId = null;
        }, 100);
    };

    const mutationObserver = new MutationObserver(() => {
        debouncedReport();
    });
    mutationObserver.observe(document.body, {
        attributes: true,
        childList: true,
        subtree: true
    });

    window.addEventListener('scroll', debouncedReport, { passive: true });

    reportPosition();
}
