# XmlParser
Недостатки:
1. Обязательный формат по строкам (каждый рассматриваемый элемент должен находится на отлельной строке). Получаемые извне (например, по API) данные
могут быть неотформатированы.
2. Нет декодинга специальных символов (&quot ("), &apos ('), &amp (&), &gt (>), &lt (<))
3. Не учтываются пробелы. Пример: <Element attrib = "qwe" />
4. Стандарт xml может предполагать использование одинарных кавычек
5. Название может быть подстрокой другого элемента или атрибута. Пример:
ищем элемент el с аттрибутом attrib
&lt;el attrib1="wrong"&gt;
Func1 вернет "wrong, хотя этот элемент вообще не подходит
