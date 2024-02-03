using Holtz.Refit.Api.IntegrationTests.Extensions;
using Holtz.Refit.Api.IntegrationTests.Setup;
using Holtz.Refit.Domain;
using System.Net.Mime;
using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Holtz.Refit.Api.IntegrationTests
{
    public class UsersTests : IntegrationTestsBase
    {
        public UsersTests(ApiWebApplicationFactory fixture) : base(fixture) { }

        [Fact]
        public async Task Get_Users_Successfully()
        {
            // Arrange
            RandomDataApiServer.Given(
                    Request.Create().WithPath("/api/v2/users").UsingGet()
                ).RespondWith(
                    Response.Create().WithStatusCode(200).WithHeader("Content-Type", MediaTypeNames.Application.Json)
                    .WithBody("[{\"id\":4508,\"uid\":\"b11fc07d-7f1d-4837-828d-c69c119d3cfe\",\"password\":\"V7Cy3oQU1f\",\"first_name\":\"Ozell\",\"last_name\":\"Heathcote\",\"username\":\"ozell.heathcote\",\"email\":\"ozell.heathcote@email.com\",\"avatar\":\"https://robohash.org/cumcupiditatelibero.png?size=300x300\\u0026set=set1\",\"gender\":\"Polygender\",\"phone_number\":\"+233 322.143.4863 x1707\",\"social_insurance_number\":\"988879888\",\"date_of_birth\":\"2001-07-08\",\"employment\":{\"title\":\"Global Representative\",\"key_skill\":\"Work under pressure\"},\"address\":{\"city\":\"North Sharenborough\",\"street_name\":\"Weimann Lodge\",\"street_address\":\"117 Barton Station\",\"zip_code\":\"83959-2204\",\"state\":\"North Dakota\",\"country\":\"United States\",\"coordinates\":{\"lat\":-45.61846783275135,\"lng\":-98.77357491076879}},\"credit_card\":{\"cc_number\":\"5491-7408-5594-7582\"},\"subscription\":{\"plan\":\"Silver\",\"status\":\"Idle\",\"payment_method\":\"Cash\",\"term\":\"Full subscription\"}},{\"id\":1381,\"uid\":\"f8b01366-a2ac-471c-beca-c4dd790575e8\",\"password\":\"qayfoSce8M\",\"first_name\":\"Yuri\",\"last_name\":\"Stroman\",\"username\":\"yuri.stroman\",\"email\":\"yuri.stroman@email.com\",\"avatar\":\"https://robohash.org/enimindolorum.png?size=300x300\\u0026set=set1\",\"gender\":\"Genderfluid\",\"phone_number\":\"+82 944.761.4927\",\"social_insurance_number\":\"563424282\",\"date_of_birth\":\"1993-03-22\",\"employment\":{\"title\":\"Marketing Manager\",\"key_skill\":\"Fast learner\"},\"address\":{\"city\":\"West Luann\",\"street_name\":\"Shelby Rest\",\"street_address\":\"3086 Lissette Glens\",\"zip_code\":\"30228-4990\",\"state\":\"Tennessee\",\"country\":\"United States\",\"coordinates\":{\"lat\":-3.320540923852974,\"lng\":119.59731237999779}},\"credit_card\":{\"cc_number\":\"6771-8973-0956-5581\"},\"subscription\":{\"plan\":\"Premium\",\"status\":\"Idle\",\"payment_method\":\"Bitcoins\",\"term\":\"Annual\"}},{\"id\":553,\"uid\":\"1e8d252f-65a5-48e5-b2cf-04fb120331ee\",\"password\":\"nvWR3bQNga\",\"first_name\":\"Alvaro\",\"last_name\":\"Corwin\",\"username\":\"alvaro.corwin\",\"email\":\"alvaro.corwin@email.com\",\"avatar\":\"https://robohash.org/illoconsequaturet.png?size=300x300\\u0026set=set1\",\"gender\":\"Non-binary\",\"phone_number\":\"+357 1-666-183-9673 x7035\",\"social_insurance_number\":\"390349264\",\"date_of_birth\":\"1973-06-11\",\"employment\":{\"title\":\"Consulting Consultant\",\"key_skill\":\"Problem solving\"},\"address\":{\"city\":\"North Marco\",\"street_name\":\"Koepp Camp\",\"street_address\":\"391 Hyatt Square\",\"zip_code\":\"97908\",\"state\":\"Kansas\",\"country\":\"United States\",\"coordinates\":{\"lat\":3.533780732643933,\"lng\":-150.67064620094078}},\"credit_card\":{\"cc_number\":\"6771-8984-9039-3213\"},\"subscription\":{\"plan\":\"Professional\",\"status\":\"Idle\",\"payment_method\":\"Credit card\",\"term\":\"Payment in advance\"}},{\"id\":6759,\"uid\":\"e7f2ae02-78cc-4af9-b032-eeb2d3a36fdd\",\"password\":\"hVuqA3xdMR\",\"first_name\":\"Jannie\",\"last_name\":\"Anderson\",\"username\":\"jannie.anderson\",\"email\":\"jannie.anderson@email.com\",\"avatar\":\"https://robohash.org/beataenamillo.png?size=300x300\\u0026set=set1\",\"gender\":\"Polygender\",\"phone_number\":\"+354 1-823-046-4305\",\"social_insurance_number\":\"362087025\",\"date_of_birth\":\"1998-11-18\",\"employment\":{\"title\":\"District Liaison\",\"key_skill\":\"Work under pressure\"},\"address\":{\"city\":\"Volkmantown\",\"street_name\":\"Bergnaum Cliffs\",\"street_address\":\"262 Caroline Burgs\",\"zip_code\":\"22849\",\"state\":\"North Dakota\",\"country\":\"United States\",\"coordinates\":{\"lat\":-25.452856482222543,\"lng\":-140.9116812989818}},\"credit_card\":{\"cc_number\":\"6771-8981-4764-4901\"},\"subscription\":{\"plan\":\"Diamond\",\"status\":\"Active\",\"payment_method\":\"Google Pay\",\"term\":\"Monthly\"}},{\"id\":6649,\"uid\":\"c8174bcf-4404-4523-98d2-5e741660c9d5\",\"password\":\"jpRL5xIaSD\",\"first_name\":\"Tennie\",\"last_name\":\"Kub\",\"username\":\"tennie.kub\",\"email\":\"tennie.kub@email.com\",\"avatar\":\"https://robohash.org/temporasuscipitconsequatur.png?size=300x300\\u0026set=set1\",\"gender\":\"Genderfluid\",\"phone_number\":\"+47 1-158-418-7922 x4981\",\"social_insurance_number\":\"475255527\",\"date_of_birth\":\"1973-06-30\",\"employment\":{\"title\":\"District Banking Supervisor\",\"key_skill\":\"Work under pressure\"},\"address\":{\"city\":\"Amieemouth\",\"street_name\":\"Berge Springs\",\"street_address\":\"62771 Jaymie Island\",\"zip_code\":\"24195-4981\",\"state\":\"Tennessee\",\"country\":\"United States\",\"coordinates\":{\"lat\":-56.99936052137067,\"lng\":-36.108297916394065}},\"credit_card\":{\"cc_number\":\"4244-9174-7128-1165\"},\"subscription\":{\"plan\":\"Starter\",\"status\":\"Active\",\"payment_method\":\"Bitcoins\",\"term\":\"Annual\"}}]")
                );

            // Act
            var response = await Client.GetAndDeserializeAsync<List<User>>("/api/users");

            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(5);
        }

        [Fact]
        public async Task Get_Beers_Error()
        {
            // Arrange
            RandomDataApiServer.Given(
                    Request.Create().WithPath("/api/v2/users").UsingGet()
                ).RespondWith(
                    Response.Create().WithStatusCode(500).WithHeader("Content-Type", MediaTypeNames.Application.Json)
                    .WithBody("{\"message\": \"Unexpected error\"}")
                );

            // Act
            var response = await Client.GetAsync("/api/beers");

            // Assert
            response.Should().NotBeNull();
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
