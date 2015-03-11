namespace UtilsFS

type System.String with
  member this.Right(index) = this.Substring(this.Length - index)
    