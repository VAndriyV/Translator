export default class TranslatorService {
    _apiBase = "/api/Translator";

    getResourcePOST = async (url, bodyData) => {
        const res = await fetch(`${this._apiBase}${url}`, {
            method: "POST",
            body: JSON.stringify(bodyData),
            headers: {
                "Content-Type": "application/json"
                // "Content-Type": "application/x-www-form-urlencoded",
            }
        });

        if (res.status === 400) {      
            var x = await res.json();
            throw (x.message + (x.analyzeErrors || ''));
        }
        return await res.json();
    };

    getResourceGET = async (url) => {
        const res = await fetch(`${this._apiBase}${url}`);

        if (!res.ok) {
            var x = await res.json();
            throw (x.message + (x.analyzeErrors || ''));
        }
        return await res.json();
    }

    getLexicalAnalyzeResult = async (sourceCode) => {
        const result = await this.getResourcePOST("/LexicalAnalyzeResult", sourceCode);

        return result;
    }

    getRecursiveAnalyzeResult = async (sourceCode) => {
        const result = await this.getResourcePOST("/RecursiceAnalyzeResult", sourceCode);

        return result;
    }

    getAutomaticAnalyzeResult = async (sourceCode) => {
        const result = await this.getResourcePOST("/AutomaticAnalyzeResult", sourceCode);

        return result;
    }

    getRelationTable = async (grammarText) => {
        const result = await this.getResourcePOST("/RelationTable", grammarText);

        return result;
    }

    getAutomaticConfiguration = async () => {
        const result = await this.getResourcePOST("/AutomaticConfiguration", "");

        return result;
    }

    getAscendingAnalyzeResult = async (sourceCode) => {
        const result = await this.getResourcePOST("/AscendingAnalyzeResult", sourceCode);

        return result;
    }

    getFullGrammar = async () => {
        const result = await this.getResourceGET("/FullGrammar");

        return result;
    }

    getExpressionGrammar = async () => {
        const result = await this.getResourceGET("/ExpressionGrammar");

        return result;
    }

    getRPNExpression = async (expression) => {
        const result = await this.getResourcePOST("/BuildExpressionRPN", expression);

        return result;
    }

    getRPNCalculation = async (variables) => {
        const result = await this.getResourcePOST("/CalculateExpressionRPN", variables);

        return result;
    }

    getFullRPN = async (sourceCode) => {
        const result = await this.getResourcePOST("/BuildRPN", sourceCode);

        return result;
    }
}