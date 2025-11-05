<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="Comparision">
				<xsl:if test="COL1 != 'account'">
					<PositionMaster>
						<!--   Fund -->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL1)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Jasincap']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select="''"/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>


						<PBAssetName>
							<xsl:value-of select="''"/>
						</PBAssetName>

						<xsl:choose>
							<xsl:when test ="COL14 &lt; 0">
								<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="COL14*(-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when test ="COL14 &gt; 0">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
								<Quantity>
									<xsl:value-of select="COL14"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="''"/>
								</Side>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name ="varGrossAmt">
							<xsl:choose>
								<xsl:when test ="COL15 &lt; 0">
									<xsl:value-of select="COL15*(-1) + (COL17 + COL19) "/>
								</xsl:when>
								<xsl:when test ="COL15 &gt; 0">
									<xsl:value-of select="COL15 - (COL17 + COL19)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="COL14 &lt; 0 ">
								<AvgPX>
									<xsl:value-of select="$varGrossAmt div COL14 *(-1)"/>
								</AvgPX>
							</xsl:when>
							<xsl:when test ="COL14 &gt; 0 ">
								<AvgPX>
									<xsl:value-of select="$varGrossAmt div COL14 "/>
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
							<xsl:when test ="boolean(number(COL19))">
								<Commission>
									<xsl:value-of select="COL19"/>
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
							<xsl:when test ="boolean(number(COL17))">
								<Fees>
									<xsl:value-of select=" COL17"/>
								</Fees>
							</xsl:when>
							<xsl:otherwise>
								<Fees>
									<xsl:value-of select="0"/>
								</Fees>
							</xsl:otherwise>
						</xsl:choose>

						<!--NET NOTIONAL-->
						<xsl:choose>
							<xsl:when test ="boolean(number(COL15))">
								<xsl:choose>
									<xsl:when test="COL15 &lt; 0">
										<NetNotionalValue>
											<xsl:value-of select="COL15*(-1)"/>
										</NetNotionalValue>
									</xsl:when>
									<xsl:when test="COL15 &gt; 0">
										<NetNotionalValue>
											<xsl:value-of select="COL15"/>
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

						<xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL12)"/>

						<CompanyName>
							<xsl:value-of select="$PB_COMPANY_NAME"/>
						</CompanyName>

						<PBSymbol>
							<xsl:value-of select="normalize-space(COL8)"/>
						</PBSymbol>


						<Symbol>
							<xsl:value-of select="normalize-space(COL8)"/>
						</Symbol >

					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
