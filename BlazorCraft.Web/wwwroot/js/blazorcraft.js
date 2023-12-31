﻿window.highlightSnippet = function () {
    Prism.highlightAll();
}
window.prettifyHtml = function format(html) {
    var tab = '  ';
    var result = '';
    var indent = '';

    html.split(/>\s*</).forEach(function (element) {
        if (element.match(/^\/\w/)) {
            indent = indent.substring(tab.length);
        }

        result += indent + '<' + element + '>\r\n';

        if (element.match(/^<?\w[^>]*[^\/]$/) && !element.startsWith("input")) {
            indent += tab;
        }
    });

    return result.substring(1, result.length - 3);
}
window.domWatcher = {
    watch: function (elementId, dotNetReference) {
        var targetNode = document.getElementById(elementId);
        var codeNode = targetNode
        // Create an observer instance linked to the callback function
        var observer = new MutationObserver(function (mutationsList, observer) {
            var innerHTML = codeNode.innerHTML;
            dotNetReference.invokeMethodAsync('OnDomChanged', innerHTML);
        });
        
        // Start observing the target node for configured mutations
        observer.observe(targetNode, {attributes: true, childList: true, subtree: true});

    }
};