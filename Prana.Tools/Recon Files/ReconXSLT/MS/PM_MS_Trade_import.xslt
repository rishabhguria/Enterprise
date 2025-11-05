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

	<xsl:template name ="MonthCodevar">
		<xsl:param name ="varMonth"/>
		<xsl:param name ="varPutCall"/>
		<xsl:choose>

			<xsl:when test ="$varMonth='01' or $varMonth='1' and $varPutCall='C'">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='02' or $varMonth='2' and $varPutCall='C'">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='03'or $varMonth='3' and $varPutCall='C'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='04' or $varMonth='4' and $varPutCall='C'">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='05' or $varMonth='5' and $varPutCall='C'">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='06' or $varMonth='6' and $varPutCall='C'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='07' or $varMonth='7' and $varPutCall='C'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='08' or $varMonth='8' and $varPutCall='C'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='09' or $varMonth='9' and $varPutCall='C'">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='10' and $varPutCall='C'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='11' and $varPutCall='C'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='12' and $varPutCall='C'">
				<xsl:value-of select ="'L'"/>
			</xsl:when>

			<xsl:when test ="$varMonth='01' or $varMonth='1'and $varPutCall='P'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='02' or $varMonth='2' and $varPutCall='P'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='03' or $varMonth='3' and $varPutCall='P'">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='04' or $varMonth='4' and $varPutCall='P'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='05'or $varMonth='5' and $varPutCall='P'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='06' or $varMonth='6' and $varPutCall='P'">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='07' or $varMonth='7' and $varPutCall='P'">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='08' or $varMonth='8' and $varPutCall='P'">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='09' or $varMonth='9' and $varPutCall='P'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='10' and $varPutCall='P'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='11' and $varPutCall='P'">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='12' and $varPutCall='P'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="varExDate"/>
		<xsl:if test="contains(substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL121),' '),' '),' '),' '),1,1),C) or contains(substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL121),' '),' '),' '),' '),1,1),P) ">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(COL121,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(COL121,'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(substring-before(substring-after(substring-after(normalize-space(COL121),' '),' '),'/'),2)"/>
				<!--<xsl:value-of select="substring-before(normalize-space(COL86),'/')"/>-->
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring-before(substring-after(substring-after(COL121,'/'),'/'),' ')"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL121),' '),' '),' '),' '),1,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<!--<xsl:value-of select =" substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL121),' '),' '),' '),' '),2)"/>-->
				<xsl:value-of select="format-number(substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL121),' '),' '),' '),' '),2),'#.00')"/>
			</xsl:variable>


			<xsl:variable name="MonthCode">
				<xsl:call-template name="MonthCodevar">
					<xsl:with-param name="varMonth" select="$ExpiryMonth"/>
					<xsl:with-param name="varPutCall" select="$PutORCall"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="Day">
				<xsl:choose>
					<xsl:when test="substring($ExpiryDay,1,1)='0'">
						<xsl:value-of select="substring($ExpiryDay,2,1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$ExpiryDay"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="ThirdFriday">
				<xsl:choose>
					<xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
						<xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
					</xsl:when>
				</xsl:choose>
			</xsl:variable>
			<!--<xsl:choose>
				<xsl:when test="number($ExpiryMonth)=1 and $ExpiryYear='15'">

				</xsl:when>

				<xsl:otherwise>-->
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCode,$StrikePrice,'D',$Day)"/>
			<!--</xsl:otherwise>-->
			<!--

			</xsl:choose>-->
		</xsl:if>
	</xsl:template>


	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select="//PositionMaster">
				<xsl:if test="number(COL34) and COL26='Trade' ">
					<PositionMaster>

						<xsl:variable name="varPositionStartDate">
							<xsl:value-of select="COL36"/>
						</xsl:variable>

						<PositionStartDate>
							<xsl:value-of select="$varPositionStartDate"/>
						</PositionStartDate>

						<xsl:variable name="varPB_Name">
							<xsl:value-of select="'MS'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL4"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$varPB_Name]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<FundName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</FundName>

						<xsl:variable name="varAvgPrice">
							<xsl:value-of select="COL71"/>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>
								<xsl:when test ="number($varAvgPrice)" >
									<xsl:value-of select ="$varAvgPrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>


						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="normalize-space(COL25)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPB_Name]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_Suffix">
							<xsl:value-of select = "substring-after(normalize-space(COL25),' ')"/>
						</xsl:variable>



						<xsl:variable name="PRANA_Exchange">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name='MS']/SymbolData[@PBSuffixCode=$PB_Suffix]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="contains(COL83,'CALLL')or contains(COL83,'PUTL') ">
									<xsl:value-of select="'Option'"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name ="Symbols" select="substring-before(COL25,' ')"/>
						<xsl:variable name="Symbol">
							<xsl:choose>
								<xsl:when test="string-length(COL25)=6 and contains(substring-after(normalize-space(COL25),' '),'HK')">
									<xsl:value-of select="concat('0',$Symbols)"/>
								</xsl:when>
								<xsl:when test="string-length(COL25)=5 and contains(substring-after(normalize-space(COL25),' '),'HK')">
									<xsl:value-of select="concat('00',$Symbols) "/>
								</xsl:when>
								<xsl:when test="string-length(COL25)=4 and contains(substring-after(normalize-space(COL25),' '),'HK')">
									<xsl:value-of select="concat('000',$Symbols)"/>
								</xsl:when>
								<!--<xsl:when test="contains(COL25,'AREIT SP')">
									<xsl:value-of select="COL25"/>
								</xsl:when>-->
								<xsl:otherwise>
									<xsl:value-of select="substring-before(COL25,' ')"/>
								</xsl:otherwise>
							</xsl:choose>
							<!--<xsl:value-of select="substring-before(COL25,' ')"/>-->
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="COL121!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Asset='Option'">
									<xsl:call-template name ="Option">
										<xsl:with-param name="Symbol" select="COL25"/>
										<xsl:with-param name="varExDate" select="COL86"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="concat($Symbol,$PRANA_Exchange)"/>
								</xsl:when>
							
								<!--<--><!--xsl:when test="$PBSuffixCode='UN' or $PBSuffixCode='US'">
									<xsl:value-of select="substring-before(COL24,' ')"/>
								</xsl:when>

								<xsl:when test="$PRANA_Exchange!=''">
									<xsl:value-of select="concat(substring-before(COL25,' '),$PRANA_Exchange)"/>
								</xsl:when>-->


									<xsl:otherwise>
										<xsl:value-of select="$PB_SYMBOL_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
						</Symbol>

						<Bloomberg>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="COL121!=''">
									<xsl:value-of select="COL121"/>
								</xsl:when>
								<xsl:when test="$Asset='Option'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Bloomberg>

						<xsl:variable name="PRANA_STRATEGY_NAME">
							<xsl:value-of select="document('../ReconMappingXml/StrategyMapping.xml')/StrategyMapping/PB[@Name=$varPB_Name]/StrategyData[@PranaFundName=$PRANA_FUND_NAME]/@PranaStrategy"/>
						</xsl:variable>


						<Strategy>
							<xsl:value-of select ="$PRANA_STRATEGY_NAME"/>
						</Strategy>



						<xsl:variable name="varQuantity">
							<xsl:value-of select="COL34"/>
						</xsl:variable>

						<NetPosition>
							<xsl:choose>
								<xsl:when test ='number($varQuantity) &lt; 0'>
									<xsl:value-of select ='$varQuantity*(-1)'/>
								</xsl:when>
								<xsl:when test ='number($varQuantity) &gt; 0'>
									<xsl:value-of select ='$varQuantity'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<!--<xsl:variable name="varSide">
							<xsl:value-of select="COL13"/>
						</xsl:variable>-->

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$Asset='Option'">
									<xsl:choose>
										<xsl:when test ="contains(COL13,'Buy')">
											<xsl:value-of select ="'A'"/>
										</xsl:when>
										<xsl:when test ="contains(COL13,'Buy to Cover')">
											<xsl:value-of select ="'B'"/>
										</xsl:when>
										<xsl:when test ="contains(COL13,'Buy Long')">
											<xsl:value-of select ="'A'"/>
										</xsl:when>
										<xsl:when test ="contains(COL13,'Sell')">
											<xsl:value-of select ="'C'"/>
										</xsl:when>
										<xsl:when test ="contains(COL13,'Sell Long')">
											<xsl:value-of select ="'C'"/>
										</xsl:when>
										<xsl:when test ="contains(COL13,'Sell Short')">
											<xsl:value-of select ="'D'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test ="contains(COL13,'Buy')">
											<xsl:value-of select ="'1'"/>
										</xsl:when>
										<xsl:when test ="contains(COL13,'Buy to Cover')">
											<xsl:value-of select ="'B'"/>
										</xsl:when>
										<xsl:when test ="contains(COL13,'Buy Long')">
											<xsl:value-of select ="'1'"/>
										</xsl:when>	
										<xsl:when test ="contains(COL13,'Sell')">
											<xsl:value-of select ="'2'"/>
										</xsl:when>
										<xsl:when test ="contains(COL13,'Sell Long')">
											<xsl:value-of select ="'2'"/>
										</xsl:when>
										<xsl:when test ="contains(COL13,'Sell Short')">
											<xsl:value-of select ="'5'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>
						<PBSymbol>
							<xsl:value-of select="COL17"/>
						</PBSymbol>
						<xsl:variable name="varCommision">
							<xsl:value-of select="COL73"/>
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
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>
						<xsl:variable name="varStamp">
							<xsl:value-of select="COL74"/>
						</xsl:variable>
						<StampDuty>
							<xsl:choose>
								<xsl:when test ='number($varStamp) &lt; 0'>
									<xsl:value-of select ='$varStamp*-1'/>
								</xsl:when>
								<xsl:when test ='number($varStamp) &gt; 0'>
									<xsl:value-of select ='$varStamp'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</StampDuty>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>

