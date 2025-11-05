<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"	>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:for-each select="Comparision">

				<xsl:if test ="COL2 !='Journal' and COL2 !='Activity Type'">

					<PositionMaster>
						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL14,'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='IB']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="normalize-space(COL14) ='MARK B. FISHER MBFAM CONSERVATIVE'">
								<FundName>
									<xsl:value-of select="'831-16278 Conservative MFisher BCS'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="normalize-space(COL14) ='MARK B. FISHER MBFAM MODERATE'">
								<FundName>
									<xsl:value-of select="'831-16276 Moderate MFisher BCS'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="normalize-space(COL14) ='NEIL ROSENFELD'">
								<FundName>
									<xsl:value-of select="'831-16338 Conservative NRosenfeld BCS'"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="COL5 &lt; 0">
								<Quantity>
									<xsl:value-of select="COL5*(-1) "/>
								</Quantity>
							</xsl:when>
							<xsl:when test ="COL5 &gt; 0">
								<Quantity>
									<xsl:value-of select="COL5 "/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="COL2='Bought'">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when>
							<xsl:when test ="COL2 = 'Sold'">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
							</xsl:when>
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="COL2"/>
								</Side>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:choose>
							<xsl:when test ="boolean(number(COL6))">
								<AvgPX>
									<xsl:value-of select="COL6"/>
								</AvgPX>
							</xsl:when>
							<xsl:otherwise>
								<AvgPX>
									<xsl:value-of select="0"/>
								</AvgPX>
							</xsl:otherwise>
						</xsl:choose>

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

						<xsl:variable name ="varNotional">
							<xsl:choose>
								<xsl:when test ="boolean(number(COL5)) and boolean(number(COL6))">
									<xsl:value-of select="COL5 * COL6"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="varNotional_1">
							<xsl:choose>
								<xsl:when test ="$varNotional &lt; 0">
									<xsl:value-of select="$varNotional *(-1)"/>
								</xsl:when>
								<xsl:when test ="$varNotional &gt; 0">
									<xsl:value-of select="$varNotional"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<!--GROSS NOTIONAL-->
						<GrossNotionalValue>
							<xsl:value-of select="$varNotional_1"/>
						</GrossNotionalValue>

						<Fees>
							<xsl:value-of select="0"/>
						</Fees>

						<!--NET NOTIONAL-->
						<xsl:choose>
							<xsl:when test ="boolean(number(COL7))">
								<xsl:choose>
									<xsl:when test="COL7 &lt; 0">
										<NetNotionalValue>
											<xsl:value-of select="COL7*(-1)"/>
										</NetNotionalValue>
										<Commission>
											<xsl:value-of select='format-number((COL7*(-1) - $varNotional_1), "###.0000")'/>
										</Commission>
									</xsl:when>
									<xsl:when test="COL7 &gt; 0">
										<NetNotionalValue>
											<xsl:value-of select="COL7"/>
										</NetNotionalValue>
										<Commission>
											<!--<xsl:value-of select="(COL7 - $varNotional_1) * (-1)"/>-->
											<xsl:value-of select='format-number((COL7 - $varNotional_1)* (-1), "###.0000")'/>
										</Commission>
									</xsl:when>
									<xsl:otherwise>
										<NetNotionalValue>
											<xsl:value-of select="0"/>
										</NetNotionalValue>
										<Commission>
											<xsl:value-of select="0"/>
										</Commission>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<NetNotionalValue>
									<xsl:value-of select="0"/>
								</NetNotionalValue>
								<Commission>
									<xsl:value-of select="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>

						<!--<SMRequest>
							<xsl:value-of select ="'TRUE'"/>
						</SMRequest>-->

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
