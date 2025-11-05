<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"
				>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="Comparision">

				<xsl:if test="(COL8 ='015A')">
					<PositionMaster>

						<!-- SYMBOL -->
						<Symbol>
							<xsl:value-of select="translate(COL19, $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
						</Symbol>
						<PBSymbol>
							<xsl:value-of select="translate(COL19,'&quot;','')"/>
						</PBSymbol>


						<!-- SIDE -->
						<xsl:variable name = "varSide" >
							<xsl:value-of select="translate(COL30,'&quot;','')"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$varSide='S'">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
							</xsl:when>
							<xsl:when test="$varSide='L'">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when>
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="''"/>
								</Side>
							</xsl:otherwise>
						</xsl:choose>


						<!--QUANTITY-->
						<xsl:choose>
							<xsl:when test="COL34 &lt; 0">
								<Quantity>
									<xsl:value-of select="COL34*(-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when test="COL34 &gt; 0">
								<Quantity>
									<xsl:value-of select="COL34"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<!--COST BASIS-->
						<xsl:choose>
							<xsl:when test ="boolean(number(COL49))">
								<AvgPX>
									<xsl:value-of select="COL49"/>
								</AvgPX>
							</xsl:when>
							<xsl:otherwise>
								<AvgPX>
									<xsl:value-of select="0"/>
								</AvgPX>
							</xsl:otherwise>
						</xsl:choose>

						<!--COMMISSION-->
						<xsl:choose>
							<xsl:when test ="boolean(number(COL51))">
								<Commission>
									<xsl:value-of select="COL51"/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>

						<!--COST BASIS-->
						<xsl:choose>
							<xsl:when test ="boolean(number(COL52))">
								<Fees>
									<xsl:value-of select="COL52"/>
								</Fees>
							</xsl:when>
							<xsl:otherwise>
								<Fees>
									<xsl:value-of select="0"/>
								</Fees>
							</xsl:otherwise>
						</xsl:choose>

						<!--GROSS NOTIONAL-->
						<xsl:choose>
							<xsl:when test ="boolean(number(COL113))">
								<xsl:choose>
									<xsl:when test="COL113 &lt; 0">
										<GrossNotionalValue>
											<xsl:value-of select="COL113*(-1)"/>
										</GrossNotionalValue>
									</xsl:when>
									<xsl:when test="COL113 &gt; 0">
										<GrossNotionalValue>
											<xsl:value-of select="COL113"/>
										</GrossNotionalValue>
									</xsl:when>
									<xsl:otherwise>
										<GrossNotionalValue>
											<xsl:value-of select="0"/>
										</GrossNotionalValue>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<GrossNotionalValue>
									<xsl:value-of select="0"/>
								</GrossNotionalValue>
							</xsl:otherwise>
						</xsl:choose>

						<!--NET NOTIONAL-->
						<xsl:choose>
							<xsl:when test ="boolean(number(COL58))">
								<xsl:choose>
									<xsl:when test="COL58 &lt; 0">
										<NetNotionalValue>
											<xsl:value-of select="COL58*(-1)"/>
										</NetNotionalValue>
									</xsl:when>
									<xsl:when test="COL58 &gt; 0">
										<NetNotionalValue>
											<xsl:value-of select="COL58"/>
										</NetNotionalValue>
									</xsl:when>
									<xsl:otherwise>
										<NetNotionalValue>
											<xsl:value-of select="0"/>
										</NetNotionalValue>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<NetNotionalValue>
									<xsl:value-of select="0"/>
								</NetNotionalValue>
							</xsl:otherwise>
						</xsl:choose>

						<!--COMPANY NAME-->
						<CompanyName>
							<xsl:value-of select="COL17"/>
						</CompanyName>

						<!--FUND NAME-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL3,'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='MS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<FundName>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->

</xsl:stylesheet>
