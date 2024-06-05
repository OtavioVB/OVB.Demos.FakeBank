using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects.Enumerators;
using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects.Exceptions;
using OVB.Demos.FakeBank.MethodResultContext;
using OVB.Demos.FakeBank.NotificationContext;
using OVB.Demos.FakeBank.NotificationContext.Interfaces;

namespace OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects;

public readonly struct DocumentValueObject
{
    public bool IsValid { get; }
    private string Document { get; }
    private TypeDocument Type { get; }
    private MethodResult<INotification> MethodResult { get; }

    private DocumentValueObject(bool isValid, string document, TypeDocument type, MethodResult<INotification> methodResult)
    {
        IsValid = isValid;
        Document = document;
        Type = type;
        MethodResult = methodResult;
    }

    public const int CnpjRequiredLength = 14;
    public const int CpfRequiredLength = 11;

    private static INotification DocumentNotificationMustBeValid(int? index = null)
        => Notification.BuildError(
            code: "DOCUMENT_MUST_BE_VALID",
            message: "O documento enviado precisa ser válido.",
            index: index);
    private static INotification CnpjDocumentNotificationMustHaveOnlyDigits(int? index = null)
        => Notification.BuildError(
            code: "CNPJ_DOCUMENT_ONLY_DIGITS",
            message: "O Cadastro Nacional da Pessoa Jurídica (CNPJ) enviado precisa conter apenas dígitos.",
            index: index);

    private static INotification CpfDocumentNotificationMustHaveOnlyDigits(int? index = null)
        => Notification.BuildError(
            code: "CPF_DOCUMENT_ONLY_DIGITS",
            message: "O Cadastro da Pessoa Física (CPF) enviado precisa conter apenas dígitos.",
            index: index);

    public static DocumentValueObject Build(
        string document, int? index = null)
    {
        if (document.Length == CnpjRequiredLength)
        {
            foreach (var character in document)
                if (!char.IsDigit(character))
                    return new DocumentValueObject(
                        isValid: false,
                        document: string.Empty,
                        type: 0,
                        methodResult: MethodResult<INotification>.BuildFailureResult(
                            notifications: [CnpjDocumentNotificationMustHaveOnlyDigits(index)]));

            return new DocumentValueObject(
                isValid: true,
                document: document,
                type: TypeDocument.CNPJ,
                methodResult: MethodResult<INotification>.BuildSuccessResult());
        }
        else if (document.Length == CpfRequiredLength)
        {
            foreach (var character in document)
                if (!char.IsDigit(character))
                    return new DocumentValueObject(
                        isValid: false,
                        document: string.Empty,
                        type: 0,
                        methodResult: MethodResult<INotification>.BuildFailureResult(
                            notifications: [CpfDocumentNotificationMustHaveOnlyDigits(index)]));

            return new DocumentValueObject(
                isValid: true,
                document: document,
                type: TypeDocument.CPF,
                methodResult: MethodResult<INotification>.BuildSuccessResult());
        }
        else return new DocumentValueObject(
            isValid: false,
            document: string.Empty,
            type: 0,
            methodResult: MethodResult<INotification>.BuildFailureResult(
                notifications: [DocumentNotificationMustBeValid(index)]));
    }

    public static DocumentValueObject Build(
        string document)
        => Build(document);

    public string GetDocument()
    {
        ValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return Document;
    }

    public TypeDocument GetTypeDocument()
    {
        ValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return Type;
    }

    public string GetTypeDocumentToString()
        => GetTypeDocument().ToString();

    public static implicit operator TypeDocument(DocumentValueObject obj)
        => obj.GetTypeDocument();
    public static implicit operator string(DocumentValueObject obj)
        => obj.GetDocument();
    public static implicit operator MethodResult<INotification>(DocumentValueObject obj)
        => obj.GetMethodResult();

    public MethodResult<INotification> GetMethodResult()
        => MethodResult;
}
