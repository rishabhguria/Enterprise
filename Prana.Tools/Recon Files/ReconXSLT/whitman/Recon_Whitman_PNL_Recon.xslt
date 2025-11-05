<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber * (-1)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
	</xsl:template>
	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>
	<xsl:template name="MonthCodeVar">
		<xsl:param name="varMonth"/>
		<xsl:choose>
			<xsl:when test="$varMonth='JAN'">
				<xsl:value-of select="'F'"/>
			</xsl:when>
			<xsl:when test="$varMonth='FEB'">
				<xsl:value-of select="'G'"/>
			</xsl:when>
			<xsl:when test="$varMonth='MAR'">
				<xsl:value-of select="'H'"/>
			</xsl:when>
			<xsl:when test="$varMonth='APR'">
				<xsl:value-of select="'J'"/>
			</xsl:when>
			<xsl:when test="$varMonth='MAY'">
				<xsl:value-of select="'K'"/>
			</xsl:when>
			<xsl:when test="$varMonth='JUN'">
				<xsl:value-of select="'M'"/>
			</xsl:when>
			<xsl:when test="$varMonth='JUL'">
				<xsl:value-of select="'N'"/>
			</xsl:when>
			<xsl:when test="$varMonth='AUG'">
				<xsl:value-of select="'Q'"/>
			</xsl:when>
			<xsl:when test="$varMonth='SEP'">
				<xsl:value-of select="'U'"/>
			</xsl:when>
			<xsl:when test="$varMonth='OCT'">
				<xsl:value-of select="'V'"/>
			</xsl:when>
			<xsl:when test="$varMonth='NOV'">
				<xsl:value-of select="'X'"/>
			</xsl:when>
			<xsl:when test="$varMonth='DEC'">
				<xsl:value-of select="'Z'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="Future">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(COL4,'FUT')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(COL6,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(substring-after(normalize-space(COL6),' '),' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(substring-after(normalize-space(COL6),' '),' '),2)"/>
			</xsl:variable>
			<xsl:variable name="MonthCode">
				<xsl:call-template name="MonthCodeVar">
					<xsl:with-param name="varMonth" select="$ExpiryMonth"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:value-of select="concat($UnderlyingSymbol,' ',$MonthCode,$ExpiryYear)"/>
		</xsl:if>
	</xsl:template>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//Comparision">
				<xsl:variable name="Cash">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL38"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Cash)">
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Grays'"/>
						</xsl:variable>
						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL7"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="contains(COL4,'FUT')">
									<xsl:value-of select="'Future'"/>
								</xsl:when>
								<xsl:when test="string-length(COL6) &gt; 20">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>	
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="Futures">
							<xsl:choose>
								<xsl:when test="$Asset='Future'">
									<xsl:choose>
										<xsl:when test="contains(COL6,' ')">
											<xsl:call-template name ="Future">
												<xsl:with-param name="Symbol" select="COL6"/>
												<xsl:with-param name="Suffix" select="''"/>
											</xsl:call-template>
										</xsl:when>
										<xsl:when test="string-length(COL6)=4">
											<xsl:value-of select="concat(substring(COL6,1,2),' ',substring(COL6,3,2))"/>
										</xsl:when>
										<xsl:when test="string-length(COL6)=5">
											<xsl:value-of select="concat(substring(COL6,1,3),' ',substring(COL6,4,2))"/>
										</xsl:when>
										<xsl:when test="string-length(COL6)=6">
											<xsl:value-of select="concat(substring(COL6,1,4),' ',substring(COL6,5,2))"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>	
						<xsl:variable name="FutureOption">
							<xsl:choose>
								<xsl:when test="contains(COL4,'FOP')">
									<xsl:choose>
										<xsl:when test="string-length(COL6)=10">
											<xsl:value-of select="concat(substring(substring-before(COL6,' '),1,2),' ',substring(substring-before(COL6,' '),3,2),substring-after(COL6,' '))"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="COL6"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="FutureOpt">
							<xsl:choose>
								<xsl:when test="contains(COL4,'FSFOP')">
										<xsl:choose>
											<xsl:when test="string-length(COL6)=10">
												<xsl:value-of select="concat(substring(substring-before(COL6,' '),1,4),' ',substring(substring-before(COL6,' '),5,2),substring-after(COL6,' '))"/>
											</xsl:when>
											<xsl:when test="string-length(COL6)=12">
												<xsl:value-of select="concat(substring(substring-before(COL6,' '),1,4),' ',substring(substring-before(COL6,' '),5,2),substring-after(COL6,' '))"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="COL6"/>
											</xsl:otherwise>
										</xsl:choose>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="Symbol">
							<xsl:value-of select="COL6"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$Asset='Future'">
									<xsl:value-of select="$Futures"/>
								</xsl:when>
								<xsl:when test="COL4='FOP'">
									<xsl:value-of select="$FutureOption"/>
								</xsl:when>
								<xsl:when test="COL4='FSFOP'">
									<xsl:value-of select="$FutureOpt"/>
								</xsl:when>
								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="string-length(COL6) &gt; 20">
									<xsl:value-of select="concat(COL6,'U')"/>
								</xsl:when>
								<xsl:when test="$Asset='Future'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="contains(COL4,'FOP')">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="contains(COL4,'FSFOP')">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDCOOptionSymbol>
						<xsl:variable name="PB_FUND_NAME" select="COL1"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
						<xsl:variable name="Year" select="substring(COL22,1,4)"/>
						<xsl:variable name="Month" select="substring(COL22,5,2)"/>
						<xsl:variable name="Day" select="substring(COL22,7,2)"/>
						<TradeDate>
							<xsl:value-of select="concat($Day,'/',$Month,'/',$Year)"/>
						</TradeDate>
						<EndingQuantity>
							<xsl:choose>
								<xsl:when test="number($Cash)">
									<xsl:value-of select="$Cash"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</EndingQuantity>
						<Currency>
							<xsl:value-of select="COL3"/>
						</Currency>
						<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>
</xsl:stylesheet>