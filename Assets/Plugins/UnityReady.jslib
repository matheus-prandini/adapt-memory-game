mergeInto(LibraryManager.library, {
  UnityReady: function () {
    if (typeof window.onUnityReady === 'function') {
      window.onUnityReady();
    }
  }
});