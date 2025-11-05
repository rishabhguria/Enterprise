<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"
				>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="Comparision">

				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="translate(translate(COL17,'&quot;',''),' ','')"/>
				</xsl:variable>

				<xsl:if test="(COL1 != 'Account Name')">
					<PositionMaster>

						<!--  SYMBOL Region right now no need for Option fileds-->
						<xsl:choose>
							<xsl:when test ="$varInstrumentType='Equity'">
								<Symbol>
									<xsl:value-of select="translate(COL16, $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="translate(COL16,'&quot;','')"/>
								</PBSymbol>
							</xsl:when>
							<xsl:when test ="$varInstrumentType='Option'">
								<xsl:variable name="varAfterQ" >
									<xsl:value-of select="translate(substring-after(COL16,'Q'),' ','')"/>
								</xsl:variable>
								<xsl:variable name = "varLength" >
									<xsl:value-of select="string-length($varAfterQ)"/>
								</xsl:variable>
								<xsl:variable name = "varAfter" >
									<xsl:value-of select="substring($varAfterQ,($varLength)-1,2)"/>
								</xsl:variable>
								<xsl:variable name = "varBefore" >
									<xsl:value-of select="substring($varAfterQ,1,($varLength)-2)"/>
								</xsl:variable>
								<Symbol>
									<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="translate(COL16,'&quot;','')"/>
								</PBSymbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="translate(COL16, $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
								</Symbol>
								<PBSymbol>
									<xsl:value-of select="translate(COL16,'&quot;','')"/>
								</PBSymbol>
							</xsl:otherwise>
						</xsl:choose>


						<!--SIDE-->
						<xsl:variable name = "varSide" >
							<xsl:value-of select="translate(COL7,'&quot;','')"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$varSide='Cover Short'">
								<Side>
									<xsl:value-of select="'Buy to Close'"/>
								</Side>
							</xsl:when>
							<xsl:when test="$varSide='Buy'">
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when>
							<xsl:when test="$varSide='Sale'">
								<Side>
									<xsl:value-of select="'Sell'"/>
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
							<xsl:when test="COL9 &lt; 0">
								<Quantity>
									<xsl:value-of select="COL9*(-1)"/>
								</Quantity>
							</xsl:when>
							<xsl:when test="COL9 &gt; 0">
								<Quantity>
									<xsl:value-of select="COL9"/>
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
							<xsl:when test ="boolean(number(COL10))">
								<AvgPX>
									<xsl:value-of select="COL10"/>
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
							<xsl:when test ="boolean(number(COL11))">
								<Commission>
									<xsl:value-of select="COL11"/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>

						<!--FEES-->
						<xsl:choose>
							<xsl:when test ="boolean(number(COL12))">
								<Fees>
									<xsl:value-of select="COL12"/>
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
							<xsl:when test ="boolean(number(COL18))">
								<xsl:choose>
									<xsl:when test="COL18 &lt; 0">
										<GrossNotionalValue>
											<xsl:value-of select="COL18*(-1)"/>
										</GrossNotionalValue>
									</xsl:when>
									<xsl:when test="COL18 &gt; 0">
										<GrossNotionalValue>
											<xsl:value-of select="COL18"/>
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
							<xsl:when test ="boolean(number(COL13))">
								<xsl:choose>
									<xsl:when test="COL13 &lt; 0">
										<NetNotionalValue>
											<xsl:value-of select="COL13*(-1)"/>
										</NetNotionalValue>
									</xsl:when>
									<xsl:when test="COL13 &gt; 0">
										<NetNotionalValue>
											<xsl:value-of select="COL13"/>
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
							<xsl:value-of select="COL6"/>
						</CompanyName>

						<!--FUND NAME-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="translate(COL1,' ','')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='UBS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

						<!--PRIME BOKER ASSET NAME-->
						<PBAssetName>
							<xsl:value-of select='$varInstrumentType'/>
						</PBAssetName>
						
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
