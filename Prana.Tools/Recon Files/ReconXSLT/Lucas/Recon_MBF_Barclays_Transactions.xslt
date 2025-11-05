<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"	>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="COL1"/>
				</xsl:variable>
				<xsl:if test ="$varInstrumentType !='Activity Date'">

					<PositionMaster>
						<!--fundname section-->
						<FundName>
							<xsl:value-of select="''"/>
						</FundName>

						<xsl:choose>
							<xsl:when test ="boolean(number(COL5))">
								<Quantity>
									<xsl:value-of select="COL5"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="COL5"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>						
						
						<Side>
							<xsl:value-of select="COL2"/>
						</Side>

						<AvgPX>
							<xsl:value-of select="COL6"/>
						</AvgPX>

						<!-- Symbol Section-->
						
						<PBSymbol>
							<xsl:value-of select="COL3"/>
						</PBSymbol>
						<Symbol>
							<xsl:value-of select="COL3"/>
						</Symbol>
						
						<CompanyName>
							<xsl:value-of select="COL4"/>
						</CompanyName>					

						<!--<SMRequest>
							<xsl:value-of select ="'TRUE'"/>
						</SMRequest>-->
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
