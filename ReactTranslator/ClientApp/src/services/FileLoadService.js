export default class FileLoadService {
    _apiBase = "/api/Translator/LoadFile";

    loadFile = async (fileName) => {
        const res = await fetch(`${this._apiBase}/${fileName}`);

        if (res.status === 400) {
            var x = await res.json();
            throw (x.message + (x.analyzeErrors || ''));
        }
        return await res.json();
    };
}