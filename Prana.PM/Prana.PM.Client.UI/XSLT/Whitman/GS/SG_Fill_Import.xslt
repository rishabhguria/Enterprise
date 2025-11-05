<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">
		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}
	</msxsl:script>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name ="varQuantity">
					<xsl:value-of select ="number(COL9)"/>
				</xsl:variable>

			
				

					<PositionMaster>
						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'SG'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL7"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>				

						

						<Symbol>

							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>


						</Symbol>


						<AccountName>
							<xsl:value-of select="'Select'"/>
						</AccountName>



						<CounterPartyID>							
							<xsl:value-of select="'15'"/>
						</CounterPartyID>

						<PositionStartDate>
							<xsl:value-of select="''"/>
						</PositionStartDate>

						<NetPosition>
							<xsl:choose>
								<xsl:when  test="number($varQuantity) &gt; 0">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>
								<xsl:when test="number($varQuantity) &lt; 0">
									<xsl:value-of select="$varQuantity* (-1)"/>
							</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>					

						
						
						<CostBasis>
							<xsl:value-of select="COL10"/>
						</CostBasis>

						<xsl:variable name="varSide" select="COL6"/>

						<SideTagValue>
							<xsl:choose>
								<xsl:when  test="$varSide='B'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when  test="$varSide='S'">
									<xsl:value-of select="'2'"/>
								</xsl:when>								
								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						

					</PositionMaster>

		
					

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>