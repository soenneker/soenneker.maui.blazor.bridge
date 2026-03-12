export class MauiBlazorBridgeInterop {

    initialize(dotNetInstance) {
        this.dotNetInstance = dotNetInstance;
    }

    observeElementPosition(element, id) {
        if (!element || !id)
            return;

        let timeoutId = null;

        // A helper function to report the element's position
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

        // Debounce the reporting function
        const debouncedReport = () => {
            if (timeoutId) {
                clearTimeout(timeoutId);
            }
            timeoutId = setTimeout(() => {
                reportPosition();
                timeoutId = null;
            }, 100); // Adjust delay as needed
        };

        // MutationObserver: watch for DOM changes that might affect layout.
        const mutationObserver = new MutationObserver(() => {
            debouncedReport();
        });
        mutationObserver.observe(document.body, {
            attributes: true,
            childList: true,
            subtree: true
        });

        // Listen to scroll events, which can also change the element's position.
        window.addEventListener('scroll', debouncedReport, { passive: true });

        // Optionally, report immediately on load.
        reportPosition();
    }
}

// Export the instance for global use
window.MauiBlazorBridgeInterop = new MauiBlazorBridgeInterop();
