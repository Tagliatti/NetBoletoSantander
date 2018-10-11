using System;
using Xunit;

namespace NetBoletoSantander.Test
{
    public class PagadorTest
    {
        private readonly dynamic _pagador; 
        
        public PagadorTest()
        {
            _pagador = new
            {
                NumeroDocumento = "515.436.942-41",
                Nome = "Fulano da Silva",
                Endereco = "Lugar qualquer",
                Bairro = "Algum bairro",
                Cidade = "Alguma cidade",
                Uf = "AC",
                Cep = "12345-678",
            };
        }
        
        [Theory]
        [InlineData("")]
        [InlineData("1231231231231231232312312")]
        [InlineData("Mussum Ipsum, cacilds vidis litro abertis")]
        public void NumeroDocumentoInvalido(string numeroDocumento)
        {
            var e = Assert.Throws<ArgumentException>(() => new Pagador(numeroDocumento, _pagador.Nome,
                _pagador.Endereco, _pagador.Bairro, _pagador.Cidade, _pagador.Uf, _pagador.Cep));

            Assert.Equal("NumeroDocumento", e.ParamName);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData("Mussum Ipsum, cacilds vidis litro abertis")]
        public void NomeInvalido(string nome)
        {
            var e = Assert.Throws<ArgumentException>(() => new Pagador(_pagador.NumeroDocumento, nome,
                _pagador.Endereco, _pagador.Bairro, _pagador.Cidade, _pagador.Uf, _pagador.Cep));

            Assert.Equal("Nome", e.ParamName);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData("Mussum Ipsum, cacilds vidis litro abertis")]
        public void EnderecoInvalido(string endereco)
        {
            var e = Assert.Throws<ArgumentException>(() => new Pagador(_pagador.NumeroDocumento, _pagador.Nome,
                endereco, _pagador.Bairro, _pagador.Cidade, _pagador.Uf, _pagador.Cep));

            Assert.Equal("Endereco", e.ParamName);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData("Mussum Ipsum, cacilds vidis litro abertis")]
        public void BairroInvalido(string bairro)
        {
            var e = Assert.Throws<ArgumentException>(() => new Pagador(_pagador.NumeroDocumento, _pagador.Nome,
                _pagador.Endereco, bairro, _pagador.Cidade, _pagador.Uf, _pagador.Cep));

            Assert.Equal("Bairro", e.ParamName);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData("Mussum Ipsum, cacilds vidis litro abertis")]
        public void CidadeInvalido(string cidade)
        {
            var e = Assert.Throws<ArgumentException>(() => new Pagador(_pagador.NumeroDocumento, _pagador.Nome,
                _pagador.Endereco, _pagador.Bairro, cidade, _pagador.Uf, _pagador.Cep));

            Assert.Equal("Cidade", e.ParamName);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData("Mussum Ipsum, cacilds vidis litro abertis")]
        public void UfInvalido(string uf)
        {
            var e = Assert.Throws<ArgumentException>(() => new Pagador(_pagador.NumeroDocumento, _pagador.Nome,
                _pagador.Endereco, _pagador.Bairro, _pagador.Cidade, uf, _pagador.Cep));

            Assert.Equal("Uf", e.ParamName);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData("Mussum Ipsum, cacilds vidis litro abertis")]
        public void CepInvalido(string cep)
        {
            var e = Assert.Throws<ArgumentException>(() => new Pagador(_pagador.NumeroDocumento, _pagador.Nome,
                _pagador.Endereco, _pagador.Bairro, _pagador.Cidade, _pagador.Uf, cep));

            Assert.Equal("Cep", e.ParamName);
        }
        
        [Fact]
        public void TipoDocumentoDeveSer01()
        {
            var pagador = new Pagador("515.436.942-41", _pagador.Nome,
                _pagador.Endereco, _pagador.Bairro, _pagador.Cidade, _pagador.Uf, _pagador.Cep);

            Assert.Equal("01", pagador.TipoDocumento);
        }
        
        [Fact]
        public void TipoDocumentoDeveSer02()
        {
            var pagador = new Pagador("24.162.802/0001-66", _pagador.Nome,
                _pagador.Endereco, _pagador.Bairro, _pagador.Cidade, _pagador.Uf, _pagador.Cep);

            Assert.Equal("02", pagador.TipoDocumento);
        }
        
        [Theory]
        [InlineData("515.436.942-41")]
        [InlineData("24.162.802/0001-66")]
        public void ConstrutorComTodosOsParametrosInformados(string numeroDocumento)
        {
            var pagador = new Pagador(numeroDocumento, _pagador.Nome,
                _pagador.Endereco, _pagador.Bairro, _pagador.Cidade, _pagador.Uf, _pagador.Cep);

            Assert.IsType<Pagador>(pagador);
        }
    }
}