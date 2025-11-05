<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">
		public string Now(int year, int month)
		{
		DateTime firstFriday= new DateTime(year, month, 1);
		while (firstFriday.DayOfWeek != DayOfWeek.Friday)
		{
		firstFriday = firstFriday.AddDays(1);
		}
		return firstFriday.ToString();
		}
	</msxsl:script>

	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select="//PositionMaster">
				<xsl:if test="number(COL19)">
					<PositionMaster>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>


						<xsl:variable name ="varPBFundName">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='CORMARK']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>



						<AccountName>
							<xsl:choose>
								<xsl:when test ="$varPBFundName=''">
									<xsl:value-of select ="COL1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$varPBFundName"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

												
				


						<PositionStartDate>
							<xsl:value-of select ="COL18"/>
						</PositionStartDate>


						<CostBasis>
							<xsl:choose>
								<xsl:when test ="number(COL19)" >
									<xsl:value-of select ="COL23 div COL19"/>
								</xsl:when>							
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<Symbol>
							<xsl:value-of select="COL4"/>
						</Symbol>


						<NetPosition>
							<xsl:choose>
								<xsl:when test ='number(COL19) &lt; 0'>
									<xsl:value-of select ='COL19*-1'/>
								</xsl:when>
								<xsl:when test ='number(COL19) &gt; 0'>
									<xsl:value-of select ='COL19'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>


						<SideTagValue>
							<xsl:choose>
								<xsl:when test ="COL6='LONG'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:when test ="COL6='SHORT'">
									<xsl:value-of select ="'5'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="'0'"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>


						<PBSymbol>
							<xsl:value-of select="COL16"/>
						</PBSymbol>





					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
