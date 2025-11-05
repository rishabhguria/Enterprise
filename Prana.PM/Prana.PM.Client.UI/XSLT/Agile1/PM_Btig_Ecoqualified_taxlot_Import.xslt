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
				<xsl:if test="number(COL7)">
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




						<!--<AccountName>
							<xsl:value-of select="''"/>
						</AccountName>-->

						<PositionStartDate>
							<xsl:value-of select ="COL2"/>
						</PositionStartDate>


						<xsl:variable name="varAvgPrice">
							<xsl:value-of select="COL8"/>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>
								<xsl:when test ="number($varAvgPrice) &gt; 0" >
									<xsl:value-of select ="$varAvgPrice"/>
								</xsl:when>
								<xsl:when test ="number($varAvgPrice) &lt; 0" >
									<xsl:value-of select ="$varAvgPrice*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>


						<!--<xsl:variable name="PBSuffixCode">
							<xsl:if test="COL2='Equity' and contains(COL4,' ')">
								<xsl:value-of select = "COL4"/>
							</xsl:if>
						</xsl:variable>

						<xsl:variable name="PB_ExchangeCODE">
							<xsl:if test="COL2='Equity' and contains(COL4,' ')">
								<xsl:value-of select="substring-after(COL4, ' ')"/>
							</xsl:if>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBSuffixCode=$PB_ExchangeCODE]/@PranaSuffixCode"/>
						</xsl:variable>-->

						<!--<xsl:choose>
							<xsl:when test="COL3  ='Option - Call'">
								<IDCOOptionSymbol>
									<xsl:value-of select="translate(concat(concat(substring(COL2,1,4),'  ',substring(COL2,5)),'U'),$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
								</IDCOOptionSymbol>-->
								<Symbol>
									<xsl:value-of select ="COL5"/>
								</Symbol>
							<!--</xsl:when>


							<xsl:otherwise>
								<IDCOOptionSymbol>
									<xsl:value-of select ="''"/>
								</IDCOOptionSymbol>
								<Symbol>
									<xsl:value-of select="translate(COL2,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
								</Symbol>
							</xsl:otherwise>-->

							<!--<xsl:choose>
									-->
							<!--<xsl:when test="$PRANA_SYMBOL_NAME=''">
										<IDCOOptionSymbol>
											<xsl:value-of select ="''"/>
										</IDCOOptionSymbol>
										<Symbol>
											<xsl:value-of select="substring-before($PBSuffixCode,' ')"/>
										</Symbol>
									</xsl:when>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<IDCOOptionSymbol>
											<xsl:value-of select ="''"/>
										</IDCOOptionSymbol>
										<Symbol>
											<xsl:value-of select="concat(substring-before($PBSuffixCode,' '),$PRANA_SYMBOL_NAME)"/>
										</Symbol>
									</xsl:when>-->
							<!--

									<xsl:otherwise>
										<IDCOOptionSymbol>
											<xsl:value-of select ="''"/>
										</IDCOOptionSymbol>
										<Symbol>
											<xsl:value-of select="translate(COL4,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
										</Symbol>
									</xsl:otherwise>

								</xsl:choose>-->

							<!--</xsl:when>-->

						<!--</xsl:choose>-->


						<xsl:variable name="varQuantity">
							<xsl:value-of select="COL7"/>
						</xsl:variable>

						<NetPosition>
							<xsl:choose>
								<xsl:when test ='number($varQuantity) &lt; 0'>
									<xsl:value-of select ='$varQuantity*-1'/>
								</xsl:when>
								<xsl:when test ='number($varQuantity) &gt; 0'>
									<xsl:value-of select ='$varQuantity'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>


						<xsl:variable name="varSide">
							<xsl:value-of select="COL4"/>
						</xsl:variable>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test ="$varSide='BUY'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:when test ="$varSide='SEL'">
									<xsl:value-of select ="'2'"/>
								</xsl:when>
									<xsl:when test ="$varSide='SSL'">
									<xsl:value-of select ="'5'"/>
								</xsl:when>
								<xsl:when test ="$varSide='BTC'">
									<xsl:value-of select ="'B'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="'0'"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>


						<PBSymbol>
							<xsl:value-of select="$varSide"/>
						</PBSymbol>

						<xsl:variable name="varCommision">
							<xsl:value-of select="COL9"/>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test ='number($varCommision) &lt; 0'>
									<xsl:value-of select ='$varCommision*-1'/>
								</xsl:when>
								<xsl:when test ='number($varCommision) &gt; 0'>
									<xsl:value-of select ='$varCommision'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="varSecFee">
							<xsl:value-of select="COL10"/>
						</xsl:variable>

						<StampDuty>
							<xsl:choose>
								<xsl:when test ='number($varSecFee) &lt; 0'>
									<xsl:value-of select ="$varSecFee*-1"/>
								</xsl:when>
								<xsl:when test ='number($varSecFee) &gt; 0'>
									<xsl:value-of select ="$varSecFee"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</StampDuty>

						<xsl:variable name="varFee">
							<xsl:value-of select="COL11"/>
						</xsl:variable>

						<Fees>
							<xsl:choose>
								<xsl:when test ='number($varFee) &lt; 0'>
									<xsl:value-of select ='$varFee*-1'/>
								</xsl:when>
								<xsl:when test ='number($varFee) &gt; 0'>
									<xsl:value-of select ='$varFee'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
							</Fees>



					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>
