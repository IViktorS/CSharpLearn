# 1.7 — enum (+ [Flags])

## Суть
`enum` — именованный набор целочисленных констант. Вместо «магических чисел» — читаемые имена. Под капотом хранится как целое (по умолчанию `int`).

```csharp
enum Status { Active, Blocked, Deleted }   // 0, 1, 2
enum HttpCode { Ok = 200, NotFound = 404 } // явные значения
```
Нумерация по умолчанию с **0**, по порядку.

## Конвертации
```csharp
(int)Status.Deleted   // 2
(Status)1             // Status.Blocked
Status.Blocked.ToString()  // "Blocked" (имя, не число)
```
Полезно: `Enum.Parse`/`TryParse`, `Enum.IsDefined`, `Enum.GetValues`/`GetNames`.

## Грабли
- **Неполная типобезопасность**: `(Status)99` компилируется и работает; печатается как число `99`; `Enum.IsDefined(...)` → false. Данные извне проверяй через `IsDefined`, в `switch` держи `default`.
- **default = 0**: под 0 полезно ставить осмысленное «нейтральное» значение (`None`/`Unknown`), чтобы дефолт не был «боевым».
- `Enum.TryParse` при неудаче → `out` = `default`(0), результат = false. **Проверяй возвращаемый bool**, а не содержимое out.

## [Flags] — набор битов
Для комбинаций (права доступа и т.п.). Значения — **степени двойки**:
```csharp
[Flags] enum Permissions { None=0, Read=1, Write=2, Execute=4 }
```
- Объединить/добавить флаг: `|`  → `p |= Write;`
- Проверить флаг: `HasFlag` или `&` → `p.HasFlag(Read)`
- Снять флаг: `& ~` → `p &= ~Write;`
- `[Flags]` не меняет работу битов — влияет на `ToString()` (печатает `"Read, Write"` вместо числа) и сигнализирует «это набор».
- Почему степени двойки: иначе биты пересекаются. `Execute=3` (0011) ложно засветит `Read=1`.
